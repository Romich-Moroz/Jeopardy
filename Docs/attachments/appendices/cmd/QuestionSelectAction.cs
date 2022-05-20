//QuestionSelectAction.cs

using Jeopardy.Core.Data.Gameplay.Contexts;
using Jeopardy.Core.Data.Quiz;
using Jeopardy.Core.Data.Quiz.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Actions
{
    [ProtoContract]
	
    public class QuestionSelectAction : GameAction
    {
        [ProtoMember(1)]
        public string SelectorNetworkUserId { get; set; } = string.Empty;
        [ProtoMember(2)]
        public int RoundId { get; set; }
        [ProtoMember(3)]
        public int CategoryId { get; set; }
        [ProtoMember(4)]
        public int QuestionId { get; set; }

        public override bool CanExecute(GameState gameState) => gameState.GameContext is SelectQuestionContext;

        public override void Execute(GameState gameState)
        {
            Question? question = gameState.Quiz.Rounds[RoundId].Categories[CategoryId].Questions[QuestionId];
            question.Unplayed = false;
            gameState.CurrentQuestion = question;

            foreach (Player player in gameState.Players.Values)
            {
                player.HasAnswerAttempt = true;
            }

            gameState.GameContext = question.QuestionType switch
            {
                QuestionType.Simple => new SimpleQuestionContext(SelectorNetworkUserId),
                //QuestionType.Auction => new AuctionQuestionContext(),
                //QuestionType.Secret => new SecretQuestionContext(),
                //QuestionType.Sponsored => new SponsoredQuestionContext(),
                _ => new SimpleQuestionContext(SelectorNetworkUserId)
            };
        }

        private QuestionSelectAction() { }
        public QuestionSelectAction(string selectorNetworkUserId, int roundId, int categoryId, int questionId)
        {
            SelectorNetworkUserId = selectorNetworkUserId;
            RoundId = roundId;
            CategoryId = categoryId;
            QuestionId = questionId;
        }
    }
}
