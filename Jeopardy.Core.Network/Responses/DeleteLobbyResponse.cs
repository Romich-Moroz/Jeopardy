using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses
{
    [ProtoContract]
    public class DeleteLobbyResponse : NetworkResponse
    {
        private DeleteLobbyResponse() => RequestType = RequestType.DeleteLobby;

        public DeleteLobbyResponse(string requestId) : this() => NetworkRequestId = requestId;
    }
}
