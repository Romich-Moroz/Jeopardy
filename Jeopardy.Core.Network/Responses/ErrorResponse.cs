using Jeopardy.Core.Network.Constants;
using Jeopardy.Core.Network.Requests;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses
{
    [ProtoContract]
    public class ErrorResponse : NetworkResponse
    {
        [ProtoMember(1)]
        public string Message { get; set; } = string.Empty;
        [ProtoMember(2)]
        public ErrorCode ErrorCode { get; set; }

        private ErrorResponse() { }

        public ErrorResponse(NetworkRequest request, ErrorCode errorCode, string message)
        {
            NetworkRequestId = request.NetworkRequestId;
            RequestType = request.RequestType;
            Message = message;
            ErrorCode = errorCode;
        }
    }
}
