using Jeopardy.Core.Data.Matchmaker;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Net;

namespace Jeopardy.Web.Matchmaker.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LobbyController : ControllerBase
    {
        private static readonly ConcurrentDictionary<Guid, MatchmakerLobbyInfo> LobbyList = new();
        private static readonly Task BackgroundEraserTask = Task.Factory.StartNew(() => ExpiredLobbyEraser());

        private readonly ILogger<LobbyController> _logger;

        public LobbyController(ILogger<LobbyController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetList")]
        public IEnumerable<MatchmakerLobbyInfo> GetLobbyList()
        {
            return LobbyList.Values;
        }

        [HttpPost("Create")]
        public Guid CreateLobby(string? lobbyName, string? organizerName, int maxPlayers, bool? isPasswordProtected)
        {
            if (maxPlayers > 0 && !string.IsNullOrWhiteSpace(lobbyName) && !string.IsNullOrWhiteSpace(organizerName) && isPasswordProtected is not null)
            {
                IPAddress? remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                MatchmakerLobbyInfo? lobbyInfo = new(lobbyName, organizerName, maxPlayers, isPasswordProtected.Value, remoteIpAddress);

                while (!LobbyList.TryAdd(lobbyInfo.Guid, lobbyInfo))
                {
                    lobbyInfo.Guid = Guid.NewGuid();
                }

                return lobbyInfo.Guid;
            }

            return Guid.Empty;
        }

        [HttpPut("UpdateExpirationDate")]
        public bool UpdateLobbyExpiration(Guid lobbyId)
        {
            if (LobbyList.TryGetValue(lobbyId, out MatchmakerLobbyInfo? lobbyInfo))
            {
                TimeSpan spanToAdd = MatchmakerLobbyInfo.LobbyInfoExpirationSpan - (lobbyInfo.LobbyExpirationDate - DateTime.Now);
                lobbyInfo.LobbyExpirationDate += spanToAdd;

                return true;
            }

            return false;
        }

        [HttpPut("UpdatePlayerCount")]
        public bool UpdateLobbyPlayerCount(Guid lobbyId, int currentPlayerCount)
        {
            if (LobbyList.TryGetValue(lobbyId, out MatchmakerLobbyInfo? lobbyInfo) && currentPlayerCount > 0 && currentPlayerCount <= lobbyInfo.MaxPlayerCount)
            {
                lobbyInfo.CurrentPlayerCount = currentPlayerCount;
                UpdateLobbyExpiration(lobbyId);
                return true;
            }

            return false;
        }

        [HttpDelete("Delete")]
        public bool DeleteLobby(Guid lobbyId)
        {
            return LobbyList.Remove(lobbyId, out _);
        }


        private static void ExpiredLobbyEraser()
        {
            while (true)
            {
                IEnumerable<Guid>? expiredIds = LobbyList.Where(info => info.Value.LobbyExpirationDate < DateTime.Now)
                    .Select(info => info.Key);

                foreach (Guid expiredId in expiredIds)
                {
                    LobbyList.TryRemove(expiredId, out _);
                }

                Thread.Sleep(MatchmakerLobbyInfo.LobbyInfoExpirationSpan);
            }
        }
    }
}
