namespace Shared.Core.Data.Quiz.Interfaces
{
    public interface IQuiz
    {
        string Name { get; }
        IList<IRound> Rounds { get; }
        IQuestion FinalQuestion { get; }
    }
}
