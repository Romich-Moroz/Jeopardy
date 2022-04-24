using Jeopardy.Core.Data.Quiz.Constants;

namespace Jeopardy.Core.Data.Quiz
{
    public class Question
    {
        public byte[] RawContent { get; set; } = Array.Empty<byte>();
        public QuestionType QuestionType { get; set; } = QuestionType.Simple;
        public ContentType ContentType { get; set; } = ContentType.Text;
        public ContentAccessType ContentAccessType { get; set; } = ContentAccessType.Embedded;
        public int Price { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public string ContentPath { get; set; } = string.Empty;
    }
}
