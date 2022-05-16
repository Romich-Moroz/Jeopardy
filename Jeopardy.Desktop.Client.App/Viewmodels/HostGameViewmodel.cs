using Jeopardy.Core.Cryptography;
using Jeopardy.Core.Data.Gameplay;
using Jeopardy.Core.Data.Matchmaker;
using Jeopardy.Core.Data.Quiz;
using Jeopardy.Core.Data.Utility;
using Jeopardy.Core.Data.Validation;
using Jeopardy.Core.Network.Requests;
using Jeopardy.Core.Network.Responses;
using Jeopardy.Core.Serialization;
using Jeopardy.Core.Wpf.Commands;
using Jeopardy.Core.Wpf.Navigation;
using Jeopardy.Core.Wpf.Viewmodels;
using Jeopardy.Desktop.Client.App.Models.Storages;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace Jeopardy.Desktop.Client.App.Viewmodels
{
    internal class HostGameViewmodel : ViewmodelBase
    {
        private readonly LobbyInfoStorage _lobbyInfoStorage;
        private readonly UserIdentityStorage _userIdentityStorage;
        private readonly MatchmakerClientStorage _matchmakerClientStorage;
        private readonly NavigationService<MainMenuViewmodel> _mainMenuNavigationService;
        private readonly NavigationService<GameLobbyViewmodel> _gameLobbyNavigationService;

        private CreateLobbyRequest? _createLobbyRequest;

        public ValidationResult ValidationResult { get; set; } = new();
        public LobbyInfo LobbyInfo => _lobbyInfoStorage.CurrentLobbyInfo;
        public GameRules GameRules => _lobbyInfoStorage.CurrentLobbyInfo.GameState.GameRules;

        public string? SelectedQuizPath { get; set; } = "F:\\test.qpck";
        public string? PlainPassword { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(SelectedQuizPath) && !string.IsNullOrWhiteSpace(LobbyInfo.LobbyName);

        public ICommand MainMenuCommand => new RelayCommand(() => _mainMenuNavigationService.Navigate(), null);
        public ICommand SelectQuizPackCommand => new RelayCommand(
            () =>
            {
                OpenFileDialog openFileDialog = new();
                openFileDialog.Filter = "Quiz pack files (*.qpck)|*.qpck";

                if (openFileDialog.ShowDialog() == true)
                {
                    SelectedQuizPath = openFileDialog.FileName;
                    OnPropertyChanged(nameof(SelectedQuizPath));
                }
            },
            null
        );

        public ICommand CreateLobbyCommand => new RelayCommand(
            async () =>
            {
                if (RunValidations())
                {
                    if (PlainPassword != null)
                    {
                        LobbyInfo.Password = new SecurePassword(PlainPassword);
                        PlainPassword = null;
                    }

                    if (SelectedQuizPath != null)
                    {
                        _lobbyInfoStorage.CurrentLobbyInfo.GameState.Quiz = BinarySerializer.DeserializeFromFile<Quiz>(SelectedQuizPath);
                        QuizPacker.Unpack(_lobbyInfoStorage.CurrentLobbyInfo.GameState.Quiz);
                    }

                    _lobbyInfoStorage.CurrentLobbyInfo.NetworkLobbyId = Guid.NewGuid().ToString();
                    _lobbyInfoStorage.CurrentLobbyInfo.GameState.Host = new Player(_userIdentityStorage.CurrentUserIdentity);

                    _createLobbyRequest = new CreateLobbyRequest(_lobbyInfoStorage.CurrentLobbyInfo);
                    await _matchmakerClientStorage.MatchmakerClient.SendRequestAsync(_createLobbyRequest);
                }
            },
            () => IsValid
        );

        public HostGameViewmodel(LobbyInfoStorage lobbyInfoStorage, UserIdentityStorage userIdentityStorage, MatchmakerClientStorage matchmakerClientStorage, NavigationService<MainMenuViewmodel> mainMenuNavigationService, NavigationService<GameLobbyViewmodel> gameLobbyNavigationService)
        {
            _lobbyInfoStorage = lobbyInfoStorage;
            _userIdentityStorage = userIdentityStorage;
            _matchmakerClientStorage = matchmakerClientStorage;
            _mainMenuNavigationService = mainMenuNavigationService;
            _gameLobbyNavigationService = gameLobbyNavigationService;
            _matchmakerClientStorage.MatchmakerClient.ResponseReceived += MatchmakerClient_ResponseReceived;
        }

        public override void Unsubscribe()
        {
            _matchmakerClientStorage.MatchmakerClient.ResponseReceived -= MatchmakerClient_ResponseReceived;
            base.Unsubscribe();
        }

        private void MatchmakerClient_ResponseReceived(object? sender, NetworkResponse e)
        {
            if (_createLobbyRequest?.NetworkRequestId == e.NetworkRequestId)
            {
                _lobbyInfoStorage.CurrentLobbyInfo.GameState.ControlledNetworkUserId = _lobbyInfoStorage.CurrentLobbyInfo.GameState.Host.NetworkUserId;
                _gameLobbyNavigationService.Navigate();
            }
        }

        private bool RunValidations()
        {
            ValidationResult result = new();

            if (Validator.IsNullOrEmpty(SelectedQuizPath))
            {
                result.FieldValidationResults.Add(FieldValidationResult.Error("Quiz pack is not selected"));
            }

            //if (!Validator.InRange(GameRules.SecretQuestionRewardMultiplier, GameRules.MinSecretQuestionRewardMultiplier, GameRules.MaxSecretQuestionRewardMultiplier))
            //{
            //    result.FieldValidationResults.Add(FieldValidationResult.Error($"Max secret question reward multiplier is out of range {GameRules.MinSecretQuestionRewardMultiplier}-{GameRules.MaxSecretQuestionRewardMultiplier}"));
            //}

            //if (!Validator.InRange(GameRules.StakeQuestionMaxStakeMultiplier, GameRules.MinStakeQuestionMaxStakeMultiplier, GameRules.MaxStakeQuestionMaxStakeMultiplier))
            //{
            //    result.FieldValidationResults.Add(FieldValidationResult.Error($"Max stake multiplier is out of range {GameRules.MinStakeQuestionMaxStakeMultiplier}-{GameRules.MaxStakeQuestionMaxStakeMultiplier}"));
            //}

            if (!Validator.InRange(GameRules.QuestionAnswerTime, GameRules.MinQuestionAnswerTime, GameRules.MaxQuestionAnswerTime))
            {
                result.FieldValidationResults.Add(FieldValidationResult.Error($"Question answer time is out of range {GameRules.MinQuestionAnswerTime}-{GameRules.MaxQuestionAnswerTime}"));
            }

            if (!Validator.InRange(GameRules.QuestionHangingTime, GameRules.MinQuestionHangingTime, GameRules.MaxQuestionHangingTime))
            {
                result.FieldValidationResults.Add(FieldValidationResult.Error($"Question hanging time is out of range {GameRules.MinQuestionHangingTime}-{GameRules.MaxQuestionHangingTime}"));
            }

            ValidationResult = result;
            OnPropertyChanged(nameof(ValidationResult));

            return !ValidationResult.HasErrors();
        }
    }
}
