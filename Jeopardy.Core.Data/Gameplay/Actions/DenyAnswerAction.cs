using Jeopardy.Core.Data.Gameplay.Contexts;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    public class DenyAnswerAction : JudgeAction
    {
        public override void Execute(GameState gameState)
        {
            var playerExist = gameState.Players.ContainsKey(AnsweringPlayerId);
            if (playerExist)
            {
                gameState.Players[AnsweringPlayerId].Score -= gameState.CurrentQuestion?.Price ?? 0;
            }

            if (!gameState.Players.Values.Any(p => p.HasAnswerAttempt))
            {
                gameState.CurrentQuestion = null;
                if (gameState.CurrentRound?.HasUnplayedCategories == true)
                {
                    gameState.GameContext = new SelectQuestionContext(playerExist ? AnsweringPlayerId : gameState.Players.First().Key);
                }
                else
                {
                    gameState.SetNextRoundOrShowWinner(AnsweringPlayerId);
                }
            }
            else if (gameState.GameContext is PlayerAnswerContext ctx)
            {
                ctx.QuestionContext.IsFirstTimeShow = false;
                gameState.GameContext = ctx.QuestionContext;
            }
        }
    }
}
