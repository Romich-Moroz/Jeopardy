using Shared.Core.Data.Quiz.Constants;

namespace Shared.Core.Data.Quiz.Interfaces
{
    public interface IQuestion
    {
        QuestionType QuestionType { get; }
        IQuestionContent QuestionContent { get; }
        int Price { get; }
        string CorrectAnswer { get; }
    }
}
