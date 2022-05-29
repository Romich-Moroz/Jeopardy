using Jeopardy.Core.Data.Quiz.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Data.Quiz
{
    [ProtoContract]
    public class Question
    {
        [ProtoMember(1)]
        public byte[] RawContent { get; set; } = Array.Empty<byte>();
        [ProtoMember(2)]
        public QuestionType QuestionType { get; set; } = QuestionType.Simple;
        [ProtoMember(3)]
        public ContentType ContentType { get; set; } = ContentType.Text;
        [ProtoMember(4)]
        public ContentAccessType ContentAccessType { get; set; } = ContentAccessType.Embedded;
        [ProtoMember(5)]
        public int Price { get; set; }
        [ProtoMember(6)]
        public string CorrectAnswer { get; set; } = string.Empty;
        [ProtoMember(7)]
        public string TaskDescription { get; set; } = string.Empty;
        [ProtoMember(8)]
        public string ContentPath { get; set; } = string.Empty;
        [ProtoMember(9)]
        public bool Unplayed { get; set; } = true;

        public override bool Equals(object? obj) => obj is Question q && q.Price == Price && q.TaskDescription == TaskDescription && q.CorrectAnswer == CorrectAnswer;
    }
}
