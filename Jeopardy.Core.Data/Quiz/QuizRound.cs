using ProtoBuf;

namespace Jeopardy.Core.Data.Quiz
{
    [ProtoContract]
    public class QuizRound
    {
        [ProtoMember(1)]
        public IList<QuizCategory> Categories { get; set; } = new List<QuizCategory>();
        [ProtoMember(2)]
        public string Name { get; set; } = "Round name";
        public bool HasUnplayedCategories => Categories.Any(c => c.HasUnplayedQuestions);

        public QuizRound() => Categories = new List<QuizCategory>();

        public QuizRound(IList<QuizCategory> quizCategories) => Categories = quizCategories;
    }
}
