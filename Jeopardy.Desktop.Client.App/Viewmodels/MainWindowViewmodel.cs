using Jeopardy.Core.Wpf.Navigation;
using Jeopardy.Core.Wpf.Viewmodels;

namespace Jeopardy.Desktop.Client.App.Viewmodels
{
    internal class MainWindowViewmodel : ViewmodelBase
    {
        private readonly NavigationStorage _navigationStorage;

        public ViewmodelBase? CurrentViewmodel => _navigationStorage.CurrentViewmodel;

        public MainWindowViewmodel(NavigationStorage navigationStorage)
        {
            _navigationStorage = navigationStorage;
            _navigationStorage.CurrentViewmodelChanged += OnCurrentViewmodelChanged;
        }

        public override void Unsubscribe()
        {
            _navigationStorage.CurrentViewmodelChanged -= OnCurrentViewmodelChanged;
            base.Unsubscribe();
        }

        private void OnCurrentViewmodelChanged() => OnPropertyChanged(nameof(CurrentViewmodel));
    }
}
