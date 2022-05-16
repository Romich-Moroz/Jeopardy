using Jeopardy.Core.Data.Gameplay.Contexts;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    public class SkipQuestionAction : GameAction
    {
        public override bool CanExecute(GameState gameState) => gameState.GameContext is QuestionContext;
        public override void Execute(GameState gameState)
        {
            if (gameState.GameContext is QuestionContext ctx)
            {
                gameState.CurrentQuestion = null;
                gameState.SetNextContext(ctx.SelectorNetworkUserId);
            }
        }
    }
}
