using Shared.Core.Data.Quiz.Constants;

namespace Shared.Core.Data.Quiz.Interfaces
{
    public interface IQuestionContent
    {
        ContentAccessType ContentAccessType { get; }
        ContentType ContentType { get; }
        string ContentLink { get; }
        byte[] ContentBinary { get; }
    }
}
