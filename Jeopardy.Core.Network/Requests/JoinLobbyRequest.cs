using Jeopardy.Core.Data.Gameplay;
using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Requests
{
    [ProtoContract]
    public class JoinLobbyRequest : NetworkRequest
    {
        [ProtoMember(1)]
        public string NetworkLobbyId { get; set; } = string.Empty;
        [ProtoMember(2)]
        public Player Player { get; set; } = new();
        [ProtoMember(3)]
        public string? Password { get; set; }

        private JoinLobbyRequest() { }

        public JoinLobbyRequest(string lobbyId, Player player, string? password)
        {
            RequestType = RequestType.JoinLobby;
            NetworkLobbyId = lobbyId;
            Player = player;
            NetworkUserId = player.NetworkUserId;
            Password = password;
        }
    }
}
