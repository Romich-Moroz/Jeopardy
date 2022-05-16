using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Contexts
{
    [ProtoContract]
    [ProtoInclude(5001, typeof(PlayerSelectContext))]
    [ProtoInclude(5002, typeof(QuestionContext))]
    [ProtoInclude(5003, typeof(WinnerContext))]
    public abstract class GameContext
    {
    }
}
