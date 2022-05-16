using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Requests
{
    [ProtoContract]
    [ProtoInclude(5001, typeof(CreateLobbyRequest))]
    [ProtoInclude(5002, typeof(DeleteLobbyRequest))]
    [ProtoInclude(5003, typeof(GetLobbyListRequest))]
    [ProtoInclude(5004, typeof(JoinLobbyRequest))]
    [ProtoInclude(5005, typeof(DisconnectRequest))]
    [ProtoInclude(5006, typeof(ExecuteGameActionRequest))]
    public abstract class NetworkRequest
    {
        [ProtoMember(1)]
        public string NetworkRequestId { get; set; } = Guid.NewGuid().ToString();
        [ProtoMember(2)]
        public RequestType RequestType { get; set; }
        [ProtoMember(3)]
        public string NetworkUserId { get; set; } = string.Empty;
    }
}
