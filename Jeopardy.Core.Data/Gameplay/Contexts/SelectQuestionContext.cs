using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Contexts
{
    [ProtoContract]
    public class SelectQuestionContext : GameContext
    {
        [ProtoMember(1)]
        public string SelectorNetworkUserId { get; set; } = string.Empty;

        private SelectQuestionContext() { }

        public SelectQuestionContext(string selectroNetworkUserId) => SelectorNetworkUserId = selectroNetworkUserId;
    }
}
