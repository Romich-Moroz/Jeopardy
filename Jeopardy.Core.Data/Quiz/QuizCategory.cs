using ProtoBuf;

namespace Jeopardy.Core.Data.Quiz
{
    [ProtoContract]
    public class QuizCategory
    {
        [ProtoMember(1)]
        public IList<Question> Questions { get; set; } = new List<Question>();
        [ProtoMember(2)]
        public string Name { get; set; } = "Category name";
        public bool HasUnplayedQuestions => Questions.Any(q => q.Unplayed);

        public QuizCategory() => Questions = new List<Question>();

        public QuizCategory(IList<Question> quizQuestions) => Questions = quizQuestions;

        public override bool Equals(object? obj) => obj is QuizCategory r && r.Questions.SequenceEqual(Questions) && r.Name == Name;
    }
}
