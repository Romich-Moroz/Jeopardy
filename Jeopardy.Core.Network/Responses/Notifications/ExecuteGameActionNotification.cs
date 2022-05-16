using Jeopardy.Core.Data.Gameplay.Actions;
using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses.Notifications
{
    [ProtoContract]
    public class ExecuteGameActionNotification : NetworkResponse
    {
        [ProtoMember(1)]
        public GameAction GameAction { get; set; }

        private ExecuteGameActionNotification() => RequestType = RequestType.NotRequested;

        public ExecuteGameActionNotification(GameAction gameAction) : this() => GameAction = gameAction;
    }
}
