using Jeopardy.Core.Data.Gameplay.Actions;
using ProtoBuf;

namespace Jeopardy.Core.Network.Requests
{
    [ProtoContract]
    public class ExecuteGameActionRequest : NetworkRequest
    {
        [ProtoMember(1)]
        public GameAction GameAction { get; set; }
        [ProtoMember(2)]
        public string NetworkLobbyId { get; set; } = string.Empty;

        private ExecuteGameActionRequest() { }

        public ExecuteGameActionRequest(string networkLobbyId, string networkUserId, GameAction gameAction)
        {
            NetworkLobbyId = networkLobbyId;
            NetworkUserId = networkUserId;
            GameAction = gameAction;
        }
    }
}
