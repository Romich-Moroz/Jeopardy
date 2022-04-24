namespace Jeopardy.Core.Data.Quiz
{
    public class QuizRound
    {
        public IList<QuizCategory> Categories { get; set; } = new List<QuizCategory>();
        public string Name { get; set; } = "Round name";

        public QuizRound()
        {
            Categories = new List<QuizCategory>();
        }

        public QuizRound(IList<QuizCategory> quizCategories)
        {
            Categories = quizCategories;
        }
    }
}
