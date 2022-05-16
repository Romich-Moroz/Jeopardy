using Jeopardy.Core.Wpf.Viewmodels;
using System;

namespace Jeopardy.Core.Wpf.Navigation
{
    public class NavigationStorage
    {
        public event Action? CurrentViewmodelChanged;

        private ViewmodelBase? _currentViewmodel;
        public ViewmodelBase? CurrentViewmodel
        {
            get => _currentViewmodel;
            set
            {
                _currentViewmodel?.Unsubscribe();
                _currentViewmodel = value;
                CurrentViewmodelChanged?.Invoke();
            }
        }
    }
}
