using Jeopardy.Core.Data.Gameplay;
using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses.Notifications
{
    [ProtoContract]
    public class PlayerJoinNotification : NetworkResponse
    {
        [ProtoMember(1)]
        public Player Player { get; set; } = new();

        private PlayerJoinNotification() => RequestType = RequestType.NotRequested;

        public PlayerJoinNotification(Player player) : this() => Player = player;
    }
}
