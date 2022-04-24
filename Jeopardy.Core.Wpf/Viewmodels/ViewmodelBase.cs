using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Jeopardy.Core.Wpf.Viewmodels
{
    public abstract class ViewmodelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
