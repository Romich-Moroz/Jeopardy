using Jeopardy.Core.Data.Quiz;
using Jeopardy.Core.Data.Quiz.Constants;
using Jeopardy.Core.Data.Validation;
using Jeopardy.Core.Files.Serialization;
using Jeopardy.Core.Localization;
using Jeopardy.Core.Wpf.Commands;
using Jeopardy.Core.Wpf.Viewmodels;
using Jeopardy.Desktop.Tester.App.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Jeopardy.Desktop.Tester.App.Viewmodels
{
    internal class MainWindowViewmodel : ViewmodelBase
    {
        public Quiz Quiz { get; set; } = new(new ObservableCollection<QuizRound>());
        public ValidationResult ValidationResult { get; set; } = new();

        public ICommand AddRoundCommand { get; }
        public ICommand RemoveRoundCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand RemoveCategoryCommand { get; }
        public ICommand AddQuestionCommand { get; }
        public ICommand RemoveQuestionCommand { get; }

        public ICommand SetContentCommand { get; }
        public ICommand SetLanguageCommand { get; }
        public ICommand RunValidationsCommand { get; }

        public ICommand NewQuizCommand { get; }
        public ICommand SaveQuizCommand { get; }
        public ICommand OpenQuizCommand { get; }

        public int SelectedRound { get; set; } = -1;
        public int SelectedCategory { get; set; } = -1;
        public int SelectedQuestion { get; set; } = -1;

        public MainWindowViewmodel()
        {
            AddRoundCommand = new RelayCommand(
                () => Quiz.Rounds.Add(new(new ObservableCollection<QuizCategory>())),
                null
            );

            AddCategoryCommand = new RelayCommand(
                () => Quiz.Rounds[SelectedRound].Categories.Add(new(new ObservableCollection<Question>())),
                IsRoundSelected
            );

            AddQuestionCommand = new RelayCommand(
                () => Quiz.Rounds[SelectedRound].Categories[SelectedCategory].Questions.Add(new Question()),
                IsCategorySelected
            );

            RemoveRoundCommand = new RelayCommand(
                () => Quiz.Rounds.RemoveAt(SelectedRound),
                IsRoundSelected
            );

            RemoveCategoryCommand = new RelayCommand(
                () => Quiz.Rounds[SelectedRound].Categories.RemoveAt(SelectedCategory),
                IsCategorySelected
            );

            RemoveQuestionCommand = new RelayCommand(
                () => Quiz.Rounds[SelectedRound].Categories[SelectedCategory].Questions.RemoveAt(SelectedQuestion),
                IsQuestionSelected
            );

            SetContentCommand = new RelayCommand(
                () =>
                {
                    OpenFileDialog openFileDialog = new();
                    Question? question = Quiz.Rounds[SelectedRound].Categories[SelectedCategory].Questions[SelectedQuestion];
                    ContentType contentType = question.ContentType;

                    switch (contentType)
                    {
                        case ContentType.Image:
                            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
                            break;
                        case ContentType.Sound:
                            openFileDialog.Filter = "Sound files (*.mp3;*.flac)|*.mp3;*.flac";
                            break;
                        case ContentType.Video:
                            openFileDialog.Filter = "Video files (*.mp4;*.avi)|*.mp4;*.avi";
                            break;
                        default:
                            return;
                    }

                    if (openFileDialog.ShowDialog() == true)
                    {

                        question.ContentPath = openFileDialog.FileName;
                        question.ContentAccessType = Core.Data.Quiz.Constants.ContentAccessType.Embedded;
                    }
                },
                null //selected question is populated before invocation so its always good
            );

            SetLanguageCommand = new RelayCommand<SupportedLocale>((locale) =>
            {
                Localizer.SetCurrentLocale(locale);
            }, null);

            RunValidationsCommand = new RelayCommand(() =>
            {
                ValidationResult = QuizPacker.Validate(Quiz);
                OnPropertyChanged(nameof(ValidationResult));
            }, null);

            NewQuizCommand = new RelayCommand(() =>
            {
                ValidationResult.FieldValidationResults.Clear();
                OnPropertyChanged(nameof(ValidationResult));
                Quiz = new(new ObservableCollection<QuizRound>());
                OnPropertyChanged(nameof(Quiz));
            }, null);

            SaveQuizCommand = new RelayCommand(() =>
            {
                SaveFileDialog dlg = new();
                dlg.Filter = "Question pack (*.qpck)|*.qpck";
                dlg.DefaultExt = "qpck";
                dlg.AddExtension = true;
                if (dlg.ShowDialog() == true)
                {
                    ValidationResult = QuizPacker.Pack(Quiz);
                    if (!ValidationResult.HasErrors())
                    {
                        BinarySerializer<Quiz>.SerializeToFile(Quiz, dlg.FileName);
                    }
                }

            }, null);

            OpenQuizCommand = new RelayCommand(() =>
            {
                OpenFileDialog openFileDialog = new();
                openFileDialog.Filter = "Question pack (*.qpck)|*.qpck";
                if (openFileDialog.ShowDialog() == true)
                {
                    Quiz = BinarySerializer<Quiz>.DeserializeFromFile(openFileDialog.FileName);
                    QuizPacker.Unpack(Quiz);
                    OnPropertyChanged(nameof(Quiz));
                }

            }, null);
        }

        private bool IsRoundSelected()
        {
            return SelectedRound >= 0 &&
                   SelectedRound < Quiz.Rounds.Count;
        }

        private bool IsCategorySelected()
        {
            return IsRoundSelected() &&
                   SelectedCategory >= 0 &&
                   SelectedCategory < Quiz.Rounds[SelectedRound].Categories.Count;
        }

        private bool IsQuestionSelected()
        {
            return IsCategorySelected() &&
                   SelectedQuestion >= 0 &&
                   SelectedQuestion < Quiz.Rounds[SelectedRound].Categories[SelectedCategory].Questions.Count;
        }
    }
}
