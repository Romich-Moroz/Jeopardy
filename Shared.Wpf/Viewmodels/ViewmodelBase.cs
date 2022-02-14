using System.ComponentModel;

namespace Shared.Wpf.Viewmodels
{
    public class ViewmodelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };
    }
}
