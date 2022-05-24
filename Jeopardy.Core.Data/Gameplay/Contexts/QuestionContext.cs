using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Contexts
{
    [ProtoContract]
    [ProtoInclude(5002, typeof(SecretQuestionContext))]
    [ProtoInclude(5003, typeof(SimpleQuestionContext))]
    [ProtoInclude(5004, typeof(SponsoredQuestionContext))]
    [ProtoInclude(5005, typeof(AuctionQuestionContext))]
    public class QuestionContext : GameContext
    {
        public string SelectorNetworkUserId { get; set; } = string.Empty;
        public bool IsFirstTimeShow { get; set; } = true;
    }
}
