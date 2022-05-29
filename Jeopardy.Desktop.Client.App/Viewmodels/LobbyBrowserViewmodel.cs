using Jeopardy.Core.Data.Gameplay;
using Jeopardy.Core.Data.Matchmaker;
using Jeopardy.Core.Network.Constants;
using Jeopardy.Core.Network.Requests;
using Jeopardy.Core.Network.Responses;
using Jeopardy.Core.Wpf.Commands;
using Jeopardy.Core.Wpf.Navigation;
using Jeopardy.Core.Wpf.Viewmodels;
using Jeopardy.Desktop.Client.App.Models.Storages;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Jeopardy.Desktop.Client.App.Viewmodels
{
    internal class LobbyBrowserViewmodel : ViewmodelBase
    {
        private readonly LobbyInfoStorage _lobbyInfoStorage;
        private readonly MatchmakerClientStorage _matchmakerClientStorage;
        private readonly UserIdentityStorage _userIdentityStorage;
        private readonly NavigationService<GameLobbyViewmodel> _gameLobbyNavigationService;
        private readonly NavigationService<MainMenuViewmodel> _mainMenuNavigationService;

        private GetLobbyListRequest? _getLobbyListRequest;
        private JoinLobbyRequest? _joinLobbyRequest;

        public List<LobbyPreview> LobbyList => _lobbyInfoStorage.BrowserLobbyList;
        public int SelectedLobbyIndex { get; set; } = -1;
        public bool ShowPasswordDialog { get; set; } = false;
        public bool ShowErrorDialog { get; set; } = false;
        public string ErrorDialogText { get; set; } = "";
        public string Password { get; set; } = string.Empty;
        public bool IsPasswordInvalid { get; set; } = false;

        public ICommand MainMenuCommand => new RelayCommand(() => _mainMenuNavigationService.Navigate(), null);
        public ICommand RefreshLobbyListCommand => new RelayCommand(
            async () =>
            {
                _getLobbyListRequest = new GetLobbyListRequest(_userIdentityStorage.CurrentUserIdentity.NetworkUserId);
                await _matchmakerClientStorage.Client.SendRequestAsync(_getLobbyListRequest);
            },
            null
        );

        public ICommand PasswordCancelCommand => new RelayCommand(
            () =>
            {
                ShowPasswordDialog = false;
                OnPropertyChanged(nameof(ShowPasswordDialog));
                Password = string.Empty;
                OnPropertyChanged(nameof(Password));
                IsPasswordInvalid = false;
                OnPropertyChanged(nameof(IsPasswordInvalid));
            },
            () => ShowPasswordDialog
        );

        public ICommand JoinLobbyCommand => new RelayCommand(
        async () =>
        {
            LobbyPreview? selectedLobby = LobbyList[SelectedLobbyIndex];

            if (selectedLobby.IsPasswordProtected && string.IsNullOrEmpty(Password))
            {
                ShowPasswordDialog = true;
                OnPropertyChanged(nameof(ShowPasswordDialog));
            }
            else
            {
                _joinLobbyRequest = new JoinLobbyRequest
                (
                    selectedLobby.NetworkLobbyId,
                    new Player(_userIdentityStorage.CurrentUserIdentity),
                    Password
                );
                await _matchmakerClientStorage.Client.SendRequestAsync(_joinLobbyRequest);
            }
        }, () => SelectedLobbyIndex >= 0);

        public LobbyBrowserViewmodel(UserIdentityStorage userIdentityStorage, LobbyInfoStorage lobbyInfoStorage, MatchmakerClientStorage matchmakerClientStorage, NavigationService<GameLobbyViewmodel> gameLobbyNavigationService, NavigationService<MainMenuViewmodel> mainMenuNavigationService)
        {
            _userIdentityStorage = userIdentityStorage;
            _lobbyInfoStorage = lobbyInfoStorage;
            _matchmakerClientStorage = matchmakerClientStorage;
            _gameLobbyNavigationService = gameLobbyNavigationService;
            _mainMenuNavigationService = mainMenuNavigationService;
            _matchmakerClientStorage.Client.ResponseReceived += MatchmakerClient_ResponseReceived;

            _lobbyInfoStorage.BrowserLobbyList = new List<LobbyPreview>();
            //RefreshLobbyListCommand.Execute(null);
        }

        public override void Unsubscribe()
        {
            _matchmakerClientStorage.Client.ResponseReceived -= MatchmakerClient_ResponseReceived;
            base.Unsubscribe();
        }

        private void MatchmakerClient_ResponseReceived(object? sender, NetworkResponse e)
        {
            switch (e)
            {
                case GetLobbyListResponse r:
                    if (e.NetworkRequestId == _getLobbyListRequest?.NetworkRequestId)
                    {
                        _lobbyInfoStorage.BrowserLobbyList = r.LobbyPreviews;
                        OnPropertyChanged(nameof(LobbyList));
                    }

                    break;
                case JoinLobbyResponse r:
                    if (e.NetworkRequestId == _joinLobbyRequest?.NetworkRequestId)
                    {
                        _lobbyInfoStorage.CurrentLobbyInfo = r.LobbyInfo;
                        _gameLobbyNavigationService.Navigate();
                    }

                    break;
                case ErrorResponse r:
                    switch (r.ErrorCode)
                    {
                        case ErrorCode.InvalidPassword:
                            IsPasswordInvalid = true;
                            OnPropertyChanged(nameof(IsPasswordInvalid));
                            break;
                        default:
                            ShowErrorDialog = true;
                            ErrorDialogText = r.Message;
                            OnPropertyChanged(nameof(ShowErrorDialog));
                            OnPropertyChanged(nameof(ErrorDialogText));
                            break;
                    }

                    break;
                default:
                    throw new InvalidOperationException($"Unexpected response of type {e.GetType()} received");
            }
        }
    }
}
