using Jeopardy.Core.Cryptography;
using Jeopardy.Core.Data.Gameplay;
using Jeopardy.Core.Data.Gameplay.Actions;
using Jeopardy.Core.Data.Gameplay.Contexts;
using Jeopardy.Core.Data.Matchmaker;
using Jeopardy.Core.Network;
using Jeopardy.Core.Network.Constants;
using Jeopardy.Core.Network.Extensions;
using Jeopardy.Core.Network.Requests;
using Jeopardy.Core.Network.Responses;
using Jeopardy.Core.Network.Responses.Notifications;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Jeopardy.Web.Matchmaker.Service
{
    public class MatchmakerService
    {
        private static readonly ConcurrentDictionary<string, LobbyInfo> _lobbyList = new();
        private static readonly IPEndPoint _serverAddress = new(IPAddress.Any, MatchmakerClient.DefaultPort);

        public static void Main()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            ClientConnectionListener();
        }

        private static void ClientConnectionListener()
        {
            LogInfo("Starting listener...");
            TcpListener? server = null;
            try
            {
                server = new TcpListener(_serverAddress);
                server.Start();

                LogInfo($"Listener started at {_serverAddress}");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    LogInfo($"Client connected {client.Client.RemoteEndPoint}");
                    ClientRequestListener(client);
                }
            }
            catch (Exception e)
            {
                LogError($"Server failed with Exception: {e}");
            }
            finally
            {
                LogWarning($"Server shutted down");
                server?.Stop();
            }
        }

        private static async void ClientRequestListener(TcpClient client)
        {
            EndPoint? clientAddress = client.Client.RemoteEndPoint;
            try
            {
                LogInfo($"Initiating request handling for {clientAddress}");
                while (client.Connected)
                {
                    LogInfo($"Waiting for requests from {clientAddress}");
                    NetworkRequest? request = await client.ReceiveDataAsync<NetworkRequest>();
                    LogInfo($"Recieved request from {clientAddress} of type {request.RequestType}");
                    NetworkResponse? response = await PrepareResponse(client, request);
                    _ = await client.SendDataAsync(response);
                    LogInfo($"Sending response to {clientAddress} of type {response.RequestType}");
                }
            }
            catch (Exception e)
            {
                LogError($"Request handling for {clientAddress} is over. Exception: {e}");
            }
            finally
            {
                foreach (LobbyInfo lobby in _lobbyList.Values)
                {
                    Player? host = lobby.GameState.Host;
                    if (host.NetworkIdentity.TcpClient?.Client.RemoteEndPoint == clientAddress)
                    {
                        LogWarning($"Host disconnected {clientAddress}, lobby deleted");
                        await NotifyEveryoneExceptSender(lobby, host.NetworkUserId, new HostDisconnectNotification());
                        _ = DeleteLobby(new DeleteLobbyRequest(host.NetworkUserId, lobby.NetworkLobbyId));
                        break;
                    }

                    foreach (Player player in lobby.GameState.Players.Values)
                    {
                        if (player.NetworkIdentity.TcpClient?.Client.RemoteEndPoint == clientAddress)
                        {
                            _ = lobby.GameState.Players.Remove(player.NetworkUserId);
                            await NotifyEveryoneExceptSender(lobby, player.NetworkUserId, new PlayerDisconnectNotification(player.NetworkUserId));
                            LogWarning($"Client disconnected {clientAddress}");
                            break;
                        }
                    }
                }

                client.Close();
            }
        }

        private static async Task<NetworkResponse> PrepareResponse(TcpClient client, NetworkRequest request) => request switch
        {
            CreateLobbyRequest r => CreateLobby(client, r),
            GetLobbyListRequest r => GetLobbyList(r),
            DeleteLobbyRequest r => DeleteLobby(r),
            JoinLobbyRequest r => await PlayerJoin(client, r),
            DisconnectRequest r => await PlayerDisconnect(r),
            ExecuteGameActionRequest r => await ExecuteGameAction(r),
            _ => new ErrorResponse(request, ErrorCode.BadRequest, $"Handling for request of type {request.GetType()} is not defined"),
        };

        private static async Task<NetworkResponse> ExecuteGameAction(ExecuteGameActionRequest request)
        {
            if (_lobbyList.TryGetValue(request.NetworkLobbyId, out LobbyInfo? lobbyInfo) && lobbyInfo is not null)
            {
                if (request.GameAction.CanExecute(lobbyInfo.GameState))
                {
                    request.GameAction.Execute(lobbyInfo.GameState);
                    await NotifyEveryoneExceptSender(lobbyInfo, request.NetworkUserId, new ExecuteGameActionNotification(request.GameAction));
                }

                return new ExecuteGameActionNotification(request.GameAction);
            }

            return new ErrorResponse(request, ErrorCode.LobbyNotFound, $"Lobby no longer exists");
        }

        private static async Task<NetworkResponse> PlayerDisconnect(DisconnectRequest request)
        {
            if (_lobbyList.TryGetValue(request.NetworkLobbyId, out LobbyInfo? lobbyInfo) && lobbyInfo is not null)
            {
                Player? host = lobbyInfo.GameState.Host;
                GameState? gameState = lobbyInfo.GameState;
                if (host.NetworkUserId == request.NetworkUserId)
                {
                    _ = DeleteLobby(new DeleteLobbyRequest(request.NetworkUserId, request.NetworkLobbyId));
                    await NotifyEveryoneExceptSender(lobbyInfo, request.NetworkUserId, new HostDisconnectNotification());
                }
                else
                {
                    _ = gameState.Players.Remove(request.NetworkUserId);
                    switch (gameState.GameContext)
                    {
                        case SelectQuestionContext c:
                            if (c.SelectorNetworkUserId == request.NetworkUserId)
                            {
                                c.SelectorNetworkUserId = gameState.Players.First().Key;
                            }

                            break;
                        case QuestionContext c:
                            if (c.SelectorNetworkUserId == request.NetworkUserId)
                            {
                                c.SelectorNetworkUserId = gameState.Players.First().Key;
                            }

                            break;
                        case PlayerAnswerContext c:
                            if (c.AnsweringPlayerId == request.NetworkUserId)
                            {
                                new DenyAnswerAction().Execute(gameState);
                            }

                            break;
                    }

                    await NotifyEveryoneExceptSender(lobbyInfo, request.NetworkUserId, new PlayerDisconnectNotification(request.NetworkUserId));
                }

                return new PlayerDisconnectNotification(request.NetworkUserId);
            }

            return new ErrorResponse(request, ErrorCode.LobbyNotFound, $"Lobby no longer exists");
        }

        private static async Task<NetworkResponse> PlayerJoin(TcpClient client, JoinLobbyRequest request)
        {
            if (_lobbyList.TryGetValue(request.NetworkLobbyId, out LobbyInfo? lobbyInfo)
                && lobbyInfo is not null)
            {
                if (!lobbyInfo.IsPasswordProtected
                    || (lobbyInfo.IsPasswordProtected
                        && lobbyInfo.Password is not null
                        && request.Password is not null
                        && lobbyInfo.Password.Verify(request.Password)))
                {
                    if (lobbyInfo.CurrentPlayerCount < lobbyInfo.MaxPlayerCount)
                    {
                        request.Player.NetworkIdentity.TcpClient = client;
                        lobbyInfo.GameState.Players.Add(request.Player.NetworkUserId, request.Player);
                        await NotifyEveryoneExceptSender(lobbyInfo, request.NetworkUserId, new PlayerJoinNotification(request.Player));
                        lobbyInfo.GameState.ControlledNetworkUserId = request.Player.NetworkUserId;
                        return new JoinLobbyResponse(request.NetworkRequestId, lobbyInfo);
                    }

                    return new ErrorResponse(request, ErrorCode.LobbyIsFull, $"Lobby is full");
                }

                return new ErrorResponse(request, ErrorCode.InvalidPassword, $"Password is invalid");
            }

            return new ErrorResponse(request, ErrorCode.LobbyNotFound, $"Lobby no longer exists");
        }

        private static async Task NotifyEveryoneExceptSender(LobbyInfo lobbyInfo, string networkSenderId, NetworkResponse networkResponse)
        {
            var connectionsToNotify = lobbyInfo.GameState.Players.Select(p => p.Value.NetworkIdentity)
                .Append(lobbyInfo.GameState.Host.NetworkIdentity)
                .Where(ni => ni.NetworkUserId != networkSenderId)
                .Select(ni => ni.TcpClient).ToList();

            foreach (TcpClient? connection in connectionsToNotify)
            {
                if (connection is not null)
                {
                    _ = await connection.SendDataAsync(networkResponse);
                }
                else
                {
                    LogWarning("Connection is null, notification ignored");
                }
            }
        }

        private static NetworkResponse GetLobbyList(GetLobbyListRequest request) =>
            new GetLobbyListResponse(
                request.NetworkRequestId,
                _lobbyList.Values.OrderByDescending(l => l.LobbyCreationDate)
                .Where(li => !li.GameState.IsStarted)
                .Select(li => li.ToLobbyPreview())
                .ToList()
            );

        private static NetworkResponse CreateLobby(TcpClient client, CreateLobbyRequest request)
        {
            request.LobbyInfo.GameState.Host.NetworkIdentity.TcpClient = client;
            if (request.Password is not null)
            {
                request.LobbyInfo.Password = new SecurePassword(request.Password);
            }

            return _lobbyList.TryAdd(request.LobbyInfo.NetworkLobbyId, request.LobbyInfo)
                ? new CreateLobbyResponse(request.NetworkRequestId)
                : new ErrorResponse(request, ErrorCode.BadRequest, "Duplicate lobby Guid, this should never happen...");
        }

        private static NetworkResponse DeleteLobby(DeleteLobbyRequest request) => _lobbyList.Remove(request.NetworkLobbyId, out _)
                ? new DeleteLobbyResponse(request.NetworkRequestId)
                : new ErrorResponse(request, ErrorCode.LobbyNotFound, $"Lobby no longer exists");

        private static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Info: {message}");
        }

        private static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Warning: {message}");
        }

        private static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {message}");
        }
    }
}
