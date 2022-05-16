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

        private CreateLobbyRequest() { }

        public CreateLobbyRequest(LobbyInfo lobbyInfo)
        {
            RequestType = RequestType.CreateLobby;
            NetworkUserId = lobbyInfo.GameState.Host.NetworkUserId;
            LobbyInfo = lobbyInfo;
        }
    }
}
