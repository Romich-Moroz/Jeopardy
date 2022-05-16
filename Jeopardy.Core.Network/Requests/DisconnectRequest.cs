using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Requests
{
    [ProtoContract]
    public class DisconnectRequest : NetworkRequest
    {
        [ProtoMember(1)]
        public string NetworkLobbyId { get; set; } = string.Empty;

        private DisconnectRequest() { }

        public DisconnectRequest(string lobbyId, string playerId)
        {
            RequestType = RequestType.Disconnect;
            NetworkUserId = playerId;
            NetworkLobbyId = lobbyId;
        }
    }
}
