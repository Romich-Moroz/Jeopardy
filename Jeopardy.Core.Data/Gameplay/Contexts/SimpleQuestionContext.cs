using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Contexts
{
    [ProtoContract]
    public class SimpleQuestionContext : QuestionContext
    {
        private SimpleQuestionContext() { }

        public SimpleQuestionContext(string selectorNetworkUserId) => SelectorNetworkUserId = selectorNetworkUserId;
    }
}
