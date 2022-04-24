namespace Jeopardy.Core.Data.Quiz
{
    public class Quiz
    {
        public IList<QuizRound> Rounds { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public Quiz()
        {
            Rounds = new List<QuizRound>();
        }

        public Quiz(IList<QuizRound> quizRounds)
        {
            Rounds = quizRounds;
        }
    }
}
