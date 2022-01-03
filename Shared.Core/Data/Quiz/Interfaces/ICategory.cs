namespace Shared.Core.Data.Quiz.Interfaces
{
    public interface ICategory
    {
        string Name { get; }
        IList<IQuestion> Questions { get; }
    }
}
