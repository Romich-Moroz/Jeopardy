using Jeopardy.Core.Data.Matchmaker;
using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Requests
{
    [ProtoContract]
    public class CreateLobbyRequest : NetworkRequest
    {
        [ProtoMember(1)]
        public LobbyInfo LobbyInfo { get; set; } = new();
        [ProtoMember(2)]
        public string? Password { get; }

        private CreateLobbyRequest() { }

        public CreateLobbyRequest(LobbyInfo lobbyInfo, string? password)
        {
            RequestType = RequestType.CreateLobby;
            NetworkUserId = lobbyInfo.GameState.Host.NetworkUserId;
            LobbyInfo = lobbyInfo;
            Password = password;
        }
    }
}
