using Jeopardy.Core.Data.Gameplay.Contexts;
using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    [ProtoContract]
    public class RequestAnswerAttemptAction : GameAction
    {
        [ProtoMember(1)]
        public string RequestorNetworkUserId { get; set; } = string.Empty;

        public override bool CanExecute(GameState gameState) => gameState.GameContext is QuestionContext;
        public override void Execute(GameState gameState)
        {
            if (gameState.GameContext is QuestionContext ctx)
            {
                gameState.GameContext = new PlayerAnswerContext { QuestionContext = ctx, AnsweringPlayerId = RequestorNetworkUserId };
                gameState.Players[RequestorNetworkUserId].HasAnswerAttempt = false;
            }
        }
    }
}
