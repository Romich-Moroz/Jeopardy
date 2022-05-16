using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses
{
    [ProtoContract]
    public class CreateLobbyResponse : NetworkResponse
    {
        private CreateLobbyResponse() => RequestType = RequestType.CreateLobby;

        public CreateLobbyResponse(string requestId) : this() => NetworkRequestId = requestId;
    }
}
