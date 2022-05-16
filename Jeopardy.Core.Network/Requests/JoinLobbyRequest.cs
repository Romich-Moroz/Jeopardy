using Jeopardy.Core.Data.Gameplay;
using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Requests
{
    [ProtoContract]
    public class JoinLobbyRequest : NetworkRequest
    {
        [ProtoMember(1)]
        public string LobbyId { get; set; } = string.Empty;
        [ProtoMember(2)]
        public Player Player { get; set; } = new();

        private JoinLobbyRequest() { }

        public JoinLobbyRequest(string lobbyId, Player player)
        {
            RequestType = RequestType.JoinLobby;
            LobbyId = lobbyId;
            Player = player;
            NetworkUserId = player.NetworkUserId;
        }
    }
}
