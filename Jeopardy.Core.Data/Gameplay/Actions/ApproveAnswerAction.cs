using Jeopardy.Core.Data.Gameplay.Contexts;
using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    [ProtoContract]
    public class ApproveAnswerAction : JudgeAction
    {
        public override void Execute(GameState gameState)
        {
            gameState.Players[AnsweringPlayerId].Score += gameState.CurrentQuestion?.Price ?? 0;

            if (gameState.CurrentRound?.HasUnplayedCategories == true)
            {
                gameState.GameContext = new SelectQuestionContext(AnsweringPlayerId);
            }
            else
            {
                gameState.SetNextRoundOrShowWinner();
            }
        }
    }
}
