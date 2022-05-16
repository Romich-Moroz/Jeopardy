namespace Jeopardy.Core.Data.Gameplay.Actions
{
    public class DenyAnswerAction : JudgeAction
    {
        public override void Execute(GameState gameState)
        {
            gameState.Players[AnsweringPlayerId].Score -= gameState.CurrentQuestion?.Price ?? 0;
            gameState.SetNextContext(AnsweringPlayerId);
        }
    }
}
