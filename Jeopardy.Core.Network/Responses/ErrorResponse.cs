using Jeopardy.Core.Network.Requests;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses
{
    [ProtoContract]
    public class ErrorResponse : NetworkResponse
    {
        private ErrorResponse() { }

        public ErrorResponse(NetworkRequest request)
        {
            NetworkRequestId = request.NetworkRequestId;
            RequestType = request.RequestType;
        }
    }
}
