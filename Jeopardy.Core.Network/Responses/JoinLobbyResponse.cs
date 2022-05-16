using Jeopardy.Core.Data.Matchmaker;
using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses
{
    [ProtoContract]
    public class JoinLobbyResponse : NetworkResponse
    {
        [ProtoMember(1)]
        public LobbyInfo LobbyInfo { get; set; } = new();

        private JoinLobbyResponse() => RequestType = RequestType.JoinLobby;

        public JoinLobbyResponse(string requestId, LobbyInfo lobbyInfo) : this()
        {
            NetworkRequestId = requestId;
            LobbyInfo = lobbyInfo;
        }
    }
}
