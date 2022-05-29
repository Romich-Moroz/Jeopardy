using Jeopardy.Core.Data.Gameplay.Contexts;
using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    [ProtoContract]
    public class StartGameAction : GameAction
    {
        public override void Execute(GameState gameState)
        {
            gameState.CurrentRound = gameState.Quiz.Rounds[0];
            gameState.GameContext = new PlayerSelectContext(gameState.Host.NetworkUserId);
            gameState.IsStarted = true;
        }

        public override bool CanExecute(GameState gameState) => gameState.CurrentRound is null && gameState.GameContext is null;
    }
}
