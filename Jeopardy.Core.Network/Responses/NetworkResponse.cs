using Jeopardy.Core.Network.Constants;
using Jeopardy.Core.Network.Responses.Notifications;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses
{
    [ProtoContract]
    [ProtoInclude(5001, typeof(CreateLobbyResponse))]
    [ProtoInclude(5002, typeof(DeleteLobbyResponse))]
    [ProtoInclude(5003, typeof(ErrorResponse))]
    [ProtoInclude(5004, typeof(GetLobbyListResponse))]
    [ProtoInclude(5005, typeof(JoinLobbyResponse))]
    [ProtoInclude(5006, typeof(PlayerJoinNotification))]
    [ProtoInclude(5007, typeof(PlayerDisconnectNotification))]
    [ProtoInclude(5008, typeof(HostDisconnectNotification))]
    [ProtoInclude(5009, typeof(ExecuteGameActionNotification))]
    public abstract class NetworkResponse
    {
        [ProtoMember(1)]
        public string NetworkRequestId { get; set; } = Guid.Empty.ToString();
        [ProtoMember(2)]
        public RequestType RequestType { get; set; }
    }
}
