using Jeopardy.Core.Data.Gameplay.Contexts;
using Jeopardy.Core.Data.Quiz;
using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay
{
    [ProtoContract]
    public class GameState
    {
        [ProtoMember(1)]
        public Quiz.Quiz Quiz { get; set; } = new();
        [ProtoMember(2)]
        public GameRules GameRules { get; set; } = new();
        [ProtoMember(3)]
        public Dictionary<string, Player> Players { get; set; } = new();
        [ProtoMember(4)]
        public Player Host { get; set; } = new Player();

        [ProtoMember(5)]
        public GameContext? GameContext { get; set; }
        [ProtoMember(6)]
        public bool IsPaused { get; set; } = true;
        [ProtoMember(7)]
        public string ControlledNetworkUserId { get; set; } = string.Empty; //0 - host, 1 - first player etc

        public string CurrentStateDescription => GameContext switch
        {
            null => "Waiting for host to start",
            PlayerSelectContext c => $"Waiting for player selection from " + (c.SelectorNetworkUserId == Host.NetworkUserId ? $"{Host.Username}" : $"{Players[c.SelectorNetworkUserId].Username}"),
            SelectQuestionContext c => $"Waiting for question selection from {Players[c.SelectorNetworkUserId].Username}",
            SimpleQuestionContext => $"Simple question with reward of {CurrentQuestion?.Price}",
            WinnerContext => $"Congratulations to winners!!!",
            //SponsoredQuestionContext => $"Sponsored question with reward of {CurrentQuestion?.Price}",
            //SecretQuestionContext => $"Secret question with price of {CurrentQuestion?.Price * GameRules.SecretQuestionRewardMultiplier}",
            //AuctionQuestionContext => $"Auction started, initial bet is {CurrentQuestion?.Price}, max bet is {CurrentQuestion?.Price * GameRules.StakeQuestionMaxStakeMultiplier}, make your bets",
            PlayerAnswerContext c => $"Player {Players[c.AnsweringPlayerId].Username} is answering",
            _ => "Description for this state is not defined"
        };

        //[ProtoMember(8)]
        //public string CurrentPlayerId { get; set; } = string.Empty; //0 - host, 1 - first player etc
        [ProtoMember(9)]
        public QuizRound? CurrentRound { get; set; }
        [ProtoMember(10)]
        public Question? CurrentQuestion { get; set; }

        public void SetNextContext(string answeringPlayerId)
        {
            Question? currentQuestion = CurrentQuestion;
            QuizRound? currentRound = CurrentRound;
            if (currentRound is not null)
            {
                if (currentQuestion is null || !Players.Values.Any(p => p.HasAnswerAttempt))
                {
                    CurrentQuestion = null;
                    if (currentRound.HasUnplayedCategories)
                    {
                        GameContext = new SelectQuestionContext(answeringPlayerId);
                    }
                    else
                    {
                        if (!SetNextRound())
                        {
                            var maxScore = Players.Values.Max(p => p.Score);
                            IEnumerable<Player>? winnerPlayers = Players.Values.Where(p => p.Score == maxScore);
                            foreach (Player? player in winnerPlayers)
                            {
                                player.IsWinner = true;
                            }
                            var winners = winnerPlayers.Select(p => p.NetworkUserId).ToList();
                            GameContext = new WinnerContext(winners);
                        }
                    }
                }
                else if (GameContext is PlayerAnswerContext ctx)
                {
                    GameContext = ctx.QuestionContext;
                }
            }
        }

        private bool SetNextRound()
        {
            QuizRound? newRound = Quiz.Rounds.FirstOrDefault(r => r.HasUnplayedCategories);
            if (newRound is not null)
            {
                CurrentRound = newRound;
                return true;
            }
            CurrentRound = null;
            return false;
        }
    }
}
