//GameAction.cs

using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    [ProtoContract]
    [ProtoInclude(5001, typeof(PlayerSelectAction))]
    [ProtoInclude(5002, typeof(StartGameAction))]
    [ProtoInclude(5003, typeof(QuestionSelectAction))]
    [ProtoInclude(5004, typeof(RequestAnswerAttemptAction))]
    [ProtoInclude(5005, typeof(JudgeAction))]
    [ProtoInclude(5006, typeof(SkipQuestionAction))]
    public abstract class GameAction
    {
        public abstract void Execute(GameState gameState);

        public abstract bool CanExecute(GameState gameState);
    }
}
