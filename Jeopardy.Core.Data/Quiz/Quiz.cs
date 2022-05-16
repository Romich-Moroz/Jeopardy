using ProtoBuf;

namespace Jeopardy.Core.Data.Quiz
{
    [ProtoContract]
    public class Quiz
    {
        [ProtoMember(1)]
        public IList<QuizRound> Rounds { get; set; } = new List<QuizRound>();
        [ProtoMember(2)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool HasUnplayedRounds => Rounds.Any(r => r.HasUnplayedCategories);

        public Quiz() { }

        public Quiz(IList<QuizRound> quizRounds) => Rounds = quizRounds;
    }
}
