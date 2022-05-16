using Jeopardy.Core.Data.Gameplay.Contexts;
using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    [ProtoContract]
    public class PlayerSelectAction : GameAction
    {
        [ProtoMember(1)]
        public string SelectedNetworkUserId { get; set; } = string.Empty;

        public override void Execute(GameState gameState) => gameState.GameContext = new SelectQuestionContext(SelectedNetworkUserId);
        public override bool CanExecute(GameState gameState) => gameState.GameContext is PlayerSelectContext;


        private PlayerSelectAction() { }
        public PlayerSelectAction(string selectedNetworkUserId) => SelectedNetworkUserId = selectedNetworkUserId;
    }
}
