using Jeopardy.Core.Wpf.Viewmodels;
using System;

namespace Jeopardy.Core.Wpf.Navigation
{
    public class NavigationService<TViewmodel>
        where TViewmodel : ViewmodelBase
    {
        private readonly NavigationStorage _navigationStorage;
        private readonly Func<TViewmodel> _createViewmodel;

        public NavigationService(NavigationStorage navigationStorage, Func<TViewmodel> createViewmodel)
        {
            _navigationStorage = navigationStorage;
            _createViewmodel = createViewmodel;
        }

        public void Navigate() => _navigationStorage.CurrentViewmodel = _createViewmodel();
    }

    public class ParameterNavigationService<TParameter, TViewmodel>
        where TViewmodel : ViewmodelBase
    {
        private readonly NavigationStorage _navigationStorage;
        private readonly Func<TParameter, TViewmodel> _createViewmodel;

        public ParameterNavigationService(NavigationStorage navigationStorage, Func<TParameter, TViewmodel> createViewmodel)
        {
            _navigationStorage = navigationStorage;
            _createViewmodel = createViewmodel;
        }

        public void Navigate(TParameter parameter) => _navigationStorage.CurrentViewmodel = _createViewmodel(parameter);
    }
}
