using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses.Notifications
{
    [ProtoContract]
    public class PlayerDisconnectNotification : NetworkResponse
    {
        [ProtoMember(1)]
        public string NetworkUserId { get; set; } = string.Empty;

        private PlayerDisconnectNotification() => RequestType = RequestType.NotRequested;

        public PlayerDisconnectNotification(string networkUserId) : this() => NetworkUserId = networkUserId;
    }
}
