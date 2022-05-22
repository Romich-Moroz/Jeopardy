using Jeopardy.Core.Data.Gameplay.Contexts;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    public class DenyAnswerAction : JudgeAction
    {
        public override void Execute(GameState gameState)
        {
            gameState.Players[AnsweringPlayerId].Score -= gameState.CurrentQuestion?.Price ?? 0;

            if (!gameState.Players.Values.Any(p => p.HasAnswerAttempt))
            {
                gameState.CurrentQuestion = null;
                if (gameState.CurrentRound?.HasUnplayedCategories == true)
                {
                    gameState.GameContext = new SelectQuestionContext(AnsweringPlayerId);
                }
                else
                {
                    gameState.SetNextRoundOrShowWinner();
                }
            }
            else if (gameState.GameContext is PlayerAnswerContext ctx)
            {
                gameState.GameContext = ctx.QuestionContext;
            }
        }
    }
}
