//JudgeAction.cs

using Jeopardy.Core.Data.Gameplay.Contexts;
using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    [ProtoContract]
	
    [ProtoInclude(5001, typeof(ApproveAnswerAction))]
    [ProtoInclude(5002, typeof(DenyAnswerAction))]
    public abstract class JudgeAction : GameAction
    {
        [ProtoMember(1)]
        public string AnsweringPlayerId { get; set; } = string.Empty;

        public override bool CanExecute(GameState gameState) => gameState.GameContext is PlayerAnswerContext;

    }
}
