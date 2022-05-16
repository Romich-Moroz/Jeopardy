using Jeopardy.Core.Data.Users;
using Jeopardy.Core.Wpf.Commands;
using Jeopardy.Core.Wpf.Navigation;
using Jeopardy.Core.Wpf.Viewmodels;
using Jeopardy.Desktop.Client.App.Models.Storages;
using System.Windows;
using System.Windows.Input;

namespace Jeopardy.Desktop.Client.App.Viewmodels
{
    internal class MainMenuViewmodel : ViewmodelBase
    {
        private readonly NavigationService<AuthViewmodel> _authNavigationService;
        private readonly NavigationService<HostGameViewmodel> _hostGameNavigationService;
        private readonly NavigationService<LobbyBrowserViewmodel> _joinGameNavigationService;
        private readonly UserIdentityStorage _userIdentityStorage;

        public UserIdentity User => _userIdentityStorage.CurrentUserIdentity;

        public ICommand ChangeIdentityCommand => new RelayCommand(() => _authNavigationService.Navigate(), null);
        public ICommand HostGameCommand => new RelayCommand(() => _hostGameNavigationService.Navigate(), null);
        public ICommand JoinGameCommand => new RelayCommand(() => _joinGameNavigationService.Navigate(), null);
        public ICommand ExitCommand => new RelayCommand(() => Application.Current.Shutdown(), null);

        public MainMenuViewmodel(
            UserIdentityStorage userIdentityStorage,
            NavigationService<HostGameViewmodel> hostGameNavigationService,
            NavigationService<AuthViewmodel> authNavigationService,
            NavigationService<LobbyBrowserViewmodel> joinGameNavigationService)
        {
            _userIdentityStorage = userIdentityStorage;
            _authNavigationService = authNavigationService;
            _hostGameNavigationService = hostGameNavigationService;
            _joinGameNavigationService = joinGameNavigationService;
        }
    }
}
