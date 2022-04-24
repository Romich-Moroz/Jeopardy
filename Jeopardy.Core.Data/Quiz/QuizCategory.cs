namespace Jeopardy.Core.Data.Quiz
{
    public class QuizCategory
    {
        public IList<Question> Questions { get; set; } = new List<Question>();
        public string Name { get; set; } = "Category name";

        public QuizCategory()
        {
            Questions = new List<Question>();
        }

        public QuizCategory(IList<Question> quizQuestions)
        {
            Questions = quizQuestions;
        }
    }
}
