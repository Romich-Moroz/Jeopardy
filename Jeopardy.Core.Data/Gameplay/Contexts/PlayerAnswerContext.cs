using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Contexts
{
    [ProtoContract]
    public class PlayerAnswerContext : GameContext
    {
        [ProtoMember(1)]
        public QuestionContext QuestionContext { get; set; }
        [ProtoMember(2)]
        public string AnsweringPlayerId { get; set; } = string.Empty;
    }
}
