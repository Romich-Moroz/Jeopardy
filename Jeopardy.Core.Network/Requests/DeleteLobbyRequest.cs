using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Requests
{
    [ProtoContract]
    public class DeleteLobbyRequest : NetworkRequest
    {
        [ProtoMember(1)]
        public string NetworkLobbyId { get; set; } = string.Empty;

        private DeleteLobbyRequest() { }

        public DeleteLobbyRequest(string userId, string lobbyId)
        {
            RequestType = RequestType.DeleteLobby;
            NetworkUserId = userId;
            NetworkLobbyId = lobbyId;
        }
    }
}
