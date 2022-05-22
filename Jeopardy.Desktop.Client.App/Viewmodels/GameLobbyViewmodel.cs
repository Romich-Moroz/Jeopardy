using Jeopardy.Core.Data.Gameplay;
using Jeopardy.Core.Data.Gameplay.Actions;
using Jeopardy.Core.Data.Gameplay.Contexts;
using Jeopardy.Core.Data.Matchmaker;
using Jeopardy.Core.Data.Quiz;
using Jeopardy.Core.Data.Quiz.Constants;
using Jeopardy.Core.Network.Requests;
using Jeopardy.Core.Network.Responses;
using Jeopardy.Core.Network.Responses.Notifications;
using Jeopardy.Core.Wpf.Commands;
using Jeopardy.Core.Wpf.Navigation;
using Jeopardy.Core.Wpf.Viewmodels;
using Jeopardy.Desktop.Client.App.Models;
using Jeopardy.Desktop.Client.App.Models.Storages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Jeopardy.Desktop.Client.App.Viewmodels
{
    internal class GameLobbyViewmodel : ViewmodelBase
    {
        private readonly NavigationService<MainMenuViewmodel> _mainMenuNavigationService;
        private readonly LobbyInfoStorage _lobbyInfoStorage;
        private readonly MatchmakerClientStorage _matchmakerClientStorage;
        private readonly QuizContentHelper _quizContentHelper = new();
        private readonly StartGameAction _startGameAction = new();
        private PlayerSelectAction _playerSelectAction = new(string.Empty);
        private QuestionSelectAction? _questionSelectAction;

        public StringBuilder ChatLog { get; set; } = new();

        public LobbyInfo LobbyInfo => _lobbyInfoStorage.CurrentLobbyInfo;
        public Player HostPlayer => GameState.Host;
        public GameState GameState => LobbyInfo.GameState;
        public GameContext? GameContext => GameState?.GameContext;
        public QuizRound? CurrentRound => GameState.CurrentRound;
        public Question? CurrentQuestion => GameState.CurrentQuestion;
        public Player ControlledPlayer => GameState.ControlledNetworkUserId == GameState.Host.NetworkUserId ? HostPlayer : GameState.Players[GameState.ControlledNetworkUserId];
        public ObservableCollection<Player> Players => new(GameState.Players.Values);
        public ObservableCollection<ObservableCollection<QuestionSelectorViewmodel>> RoundQuestions => CreateCurrentRoundQuestions();
        public string? QuestionHint => CurrentQuestion?.TaskDescription;
        public bool IsQuestionBoardVisible => GameContext is SelectQuestionContext;
        public bool IsQuestionContentVisible => GameContext is QuestionContext || GameContext is PlayerAnswerContext;
        public bool IsTextContent => IsQuestionContentVisible && CurrentQuestion?.ContentType == ContentType.Text;
        public bool IsImageContent => IsQuestionContentVisible && CurrentQuestion?.ContentType == ContentType.Image;
        public bool IsSoundContent => CurrentQuestion?.ContentType == ContentType.Sound;
        public bool IsMediaContent => IsQuestionContentVisible && (CurrentQuestion?.ContentType == ContentType.Video || IsSoundContent);
        public bool IsUserHost => ControlledPlayer.NetworkUserId == GameState.Host.NetworkUserId;
        public bool IsUserNotHost => !IsUserHost;
        public bool CanAnswerQuestion => IsUserNotHost && GameContext is QuestionContext && ControlledPlayer.HasAnswerAttempt;
        public bool CanStartGame => GameState.CurrentRound is null && GameState.GameContext is null && IsUserHost;
        public bool CanJudge => IsUserHost && GameContext is PlayerAnswerContext;
        public int RoundMaxQuestions => CurrentRound?.Categories.Max(c => c.Questions.Count) + 1 ?? 0;
        public int RoundCategories => CurrentRound?.Categories.Count ?? 0;
        public bool IsWinnersVisible => GameContext is WinnerContext;
        public bool CanSkipQuestion => IsUserHost && GameContext is QuestionContext;

        public ICommand DisconnectCommand => new RelayCommand(
            async () =>
            {
                await _matchmakerClientStorage.MatchmakerClient.SendRequestAsync(new DisconnectRequest(LobbyInfo.NetworkLobbyId, ControlledPlayer.NetworkUserId));
                DisconnectCleanup();
            },
            null
        );

        public ICommand ApproveAnswerCommand => new RelayCommand(
            async () =>
            {
                if (GameContext is PlayerAnswerContext ctx)
                {
                    await _matchmakerClientStorage.MatchmakerClient.SendRequestAsync(
                    new ExecuteGameActionRequest(
                        LobbyInfo.NetworkLobbyId,
                        ControlledPlayer.NetworkUserId,
                        new ApproveAnswerAction
                        {
                            AnsweringPlayerId = ctx.AnsweringPlayerId
                        }
                    ));
                }
            },
            () => CanJudge
        );

        public ICommand DeclineAnswerCommand => new RelayCommand(
            async () =>
            {
                if (GameContext is PlayerAnswerContext ctx)
                {
                    await _matchmakerClientStorage.MatchmakerClient.SendRequestAsync(
                    new ExecuteGameActionRequest(
                        LobbyInfo.NetworkLobbyId,
                        ControlledPlayer.NetworkUserId,
                        new DenyAnswerAction
                        {
                            AnsweringPlayerId = ctx.AnsweringPlayerId
                        }
                    ));
                }
            },
            () => CanJudge
        );

        public ICommand SkipQuestionCommand => new RelayCommand(
            async () =>
            {
                await _matchmakerClientStorage.MatchmakerClient.SendRequestAsync(
                    new ExecuteGameActionRequest(
                        LobbyInfo.NetworkLobbyId,
                        ControlledPlayer.NetworkUserId,
                        new SkipQuestionAction()
                    ));
            },
            () => CanSkipQuestion
        );

        public ICommand AnswerQuestionCommand => new RelayCommand(
            async () =>
            {
                await _matchmakerClientStorage.MatchmakerClient.SendRequestAsync(
                    new ExecuteGameActionRequest(
                        LobbyInfo.NetworkLobbyId,
                        ControlledPlayer.NetworkUserId,
                        new RequestAnswerAttemptAction
                        {
                            RequestorNetworkUserId = ControlledPlayer.NetworkUserId
                        }
                    )
                );
            },
            () => CanAnswerQuestion
        );

        public ICommand SelectQuestionCommand => new RelayCommand<Question>(
            async (question) =>
            {
                QuizRound? currentRound = GameState.CurrentRound;
                if (currentRound is not null && question is not null)
                {
                    var roundId = GameState.Quiz.Rounds.IndexOf(currentRound);
                    QuizCategory category = GameState.Quiz.Rounds[roundId].Categories.Single(c => c.Questions.Contains(question));
                    var categoryId = currentRound.Categories.IndexOf(category);
                    var questionId = category.Questions.IndexOf(question);

                    _questionSelectAction = new QuestionSelectAction(GameState.ControlledNetworkUserId, roundId, categoryId, questionId);

                    if (_questionSelectAction.CanExecute(GameState))
                    {
                        await _matchmakerClientStorage.MatchmakerClient.SendRequestAsync(
                        new ExecuteGameActionRequest
                        (
                            LobbyInfo.NetworkLobbyId,
                            ControlledPlayer.NetworkUserId,
                            _questionSelectAction
                        ));
                    }
                }
            },
            (question) => GameContext is SelectQuestionContext c && GameState.ControlledNetworkUserId == c.SelectorNetworkUserId
        );

        public ICommand SelectPlayerCommand => new RelayCommand<string>(
            async (networkUserId) =>
            {
                if (networkUserId is not null)
                {
                    _playerSelectAction = new PlayerSelectAction(networkUserId);
                    if (_playerSelectAction.CanExecute(GameState))
                    {
                        await _matchmakerClientStorage.MatchmakerClient.SendRequestAsync(
                        new ExecuteGameActionRequest
                        (
                            LobbyInfo.NetworkLobbyId,
                            ControlledPlayer.NetworkUserId,
                            _playerSelectAction
                        ));
                    }
                }
            },
            (index) => GameContext is PlayerSelectContext c && c.SelectorNetworkUserId == ControlledPlayer.NetworkUserId
        );

        public ICommand StartGameCommand => new RelayCommand(
            async () =>
            {
                await _matchmakerClientStorage.MatchmakerClient.SendRequestAsync(
                    new ExecuteGameActionRequest
                    (
                        LobbyInfo.NetworkLobbyId,
                        ControlledPlayer.NetworkUserId,
                        _startGameAction
                    )
                );
            },
            () => CanStartGame
        );

        public GameLobbyViewmodel(LobbyInfoStorage lobbyInfoStorage, MatchmakerClientStorage matchmakerClientStorage, NavigationService<MainMenuViewmodel> mainMenuNavigationService)
        {
            _lobbyInfoStorage = lobbyInfoStorage;
            _mainMenuNavigationService = mainMenuNavigationService;
            _matchmakerClientStorage = matchmakerClientStorage;
            _matchmakerClientStorage.MatchmakerClient.ResponseReceived += MatchmakerClient_ResponseReceived;
            _quizContentHelper.SaveToTempLocation(GameState.Quiz);
        }

        public override void Unsubscribe()
        {
            _matchmakerClientStorage.MatchmakerClient.ResponseReceived -= MatchmakerClient_ResponseReceived;
            _quizContentHelper.Cleanup();
            base.Unsubscribe();
        }

        private ObservableCollection<ObservableCollection<QuestionSelectorViewmodel>> CreateCurrentRoundQuestions()
        {
            QuizRound? round = CurrentRound;

            if (round is not null)
            {
                var result = new ObservableCollection<ObservableCollection<QuestionSelectorViewmodel>>();

                foreach (QuizCategory? category in round.Categories)
                {
                    var categoryResult = new ObservableCollection<QuestionSelectorViewmodel>
                    {
                        new QuestionSelectorViewmodel{ Question = null, Text = category.HasUnplayedQuestions ? category.Name : "", IsEnabled = category.HasUnplayedQuestions }
                    };

                    foreach (Question? question in category.Questions)
                    {
                        categoryResult.Add(new QuestionSelectorViewmodel { Question = question, Text = question.Unplayed ? question.Price.ToString() : "", IsEnabled = question.Unplayed });
                    }

                    result.Add(categoryResult);
                }

                return result;
            }

            return new ObservableCollection<ObservableCollection<QuestionSelectorViewmodel>>();
        }

        private void MatchmakerClient_ResponseReceived(object? sender, NetworkResponse e)
        {
            switch (e)
            {
                case PlayerJoinNotification n:
                    GameState.Players.Add(n.Player.NetworkUserId, n.Player);
                    LogAction($"{n.Player.NetworkIdentity.Username} joined");
                    break;
                case PlayerDisconnectNotification n:
                    Player player = GameState.Players[n.NetworkUserId];
                    _lobbyInfoStorage.CurrentLobbyInfo.GameState.Players.Remove(n.NetworkUserId);
                    LogAction($"{player.NetworkIdentity.Username} disconnected");
                    break;
                case HostDisconnectNotification:
                    DisconnectCleanup();
                    break;
                case ExecuteGameActionNotification n:
                    if (n.GameAction.CanExecute(GameState))
                    {
                        n.GameAction.Execute(GameState);
                        LogAction(GameState.CurrentStateDescription);
                        switch (n.GameAction)
                        {
                            case JudgeAction:
                            case QuestionSelectAction:
                            case SkipQuestionAction:
                                OnPropertyChanged(nameof(CurrentQuestion));
                                break;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Game state mismatch");
                    }
                    break;
            }
            NotifyAllPropeties();
        }

        private void NotifyAllPropeties()
        {
            OnPropertyChanged(nameof(GameState));
            OnPropertyChanged(nameof(GameContext));
            OnPropertyChanged(nameof(CurrentRound));
            OnPropertyChanged(nameof(IsQuestionBoardVisible));
            OnPropertyChanged(nameof(IsQuestionContentVisible));
            OnPropertyChanged(nameof(IsTextContent));
            OnPropertyChanged(nameof(IsImageContent));
            OnPropertyChanged(nameof(IsMediaContent));
            OnPropertyChanged(nameof(IsSoundContent));
            OnPropertyChanged(nameof(QuestionHint));
            OnPropertyChanged(nameof(IsUserHost));
            OnPropertyChanged(nameof(IsUserNotHost));
            OnPropertyChanged(nameof(CanAnswerQuestion));
            OnPropertyChanged(nameof(CanStartGame));
            OnPropertyChanged(nameof(CanJudge));
            OnPropertyChanged(nameof(Players));
            OnPropertyChanged(nameof(RoundMaxQuestions));
            OnPropertyChanged(nameof(IsWinnersVisible));
            OnPropertyChanged(nameof(CanSkipQuestion));
            OnPropertyChanged(nameof(RoundQuestions));
        }

        private void DisconnectCleanup()
        {
            _lobbyInfoStorage.CurrentLobbyInfo = new();
            _mainMenuNavigationService.Navigate();
        }

        private void LogAction(string message)
        {
            ChatLog.AppendLine($"{DateTime.Now.ToString("HH:mm")} {message}");
            OnPropertyChanged(nameof(ChatLog));
        }
    }
}
