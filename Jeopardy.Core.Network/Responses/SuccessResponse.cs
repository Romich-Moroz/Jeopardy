using Jeopardy.Core.Network.Requests;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses
{
    [ProtoContract]
    public class SuccessResponse : NetworkResponse
    {
        private SuccessResponse() { }

        public SuccessResponse(NetworkRequest request)
        {
            NetworkRequestId = request.NetworkRequestId;
            RequestType = request.RequestType;
        }
    }
}
