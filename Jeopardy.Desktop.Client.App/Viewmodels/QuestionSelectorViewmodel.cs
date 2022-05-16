using Jeopardy.Core.Data.Quiz;

namespace Jeopardy.Desktop.Client.App.Viewmodels
{
    public class QuestionSelectorViewmodel
    {
        public Question? Question { get; set; }
        public string Text { get; set; }
        public bool IsEnabled { get; set; }
    }
}
