using Jeopardy.Core.Data.Quiz;
using Jeopardy.Core.Data.Quiz.Constants;
using Jeopardy.Core.Data.Utility;
using Jeopardy.Core.Data.Validation;
using Jeopardy.Core.Serialization;
using Jeopardy.Core.Wpf.Commands;
using Jeopardy.Core.Wpf.Viewmodels;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Jeopardy.Desktop.Tester.App.Viewmodels
{
    internal class MainWindowViewmodel : ViewmodelBase
    {
        public Quiz Quiz { get; set; } = new(new ObservableCollection<QuizRound>());
        public ValidationResult ValidationResult { get; set; } = new();

        public ICommand AddRoundCommand => new RelayCommand(
            () => Quiz.Rounds.Add(new(new ObservableCollection<QuizCategory>())),
            null
        );

        public ICommand AddCategoryCommand => new RelayCommand(
            () => Quiz.Rounds[SelectedRound].Categories.Add(new(new ObservableCollection<Question>())),
            IsRoundSelected
        );

        public ICommand AddQuestionCommand => new RelayCommand(
            () => Quiz.Rounds[SelectedRound].Categories[SelectedCategory].Questions.Add(new Question()),
            IsCategorySelected
        );

        public ICommand RemoveRoundCommand => new RelayCommand(
            () => Quiz.Rounds.RemoveAt(SelectedRound),
            IsRoundSelected
        );

        public ICommand RemoveCategoryCommand => new RelayCommand(
            () => Quiz.Rounds[SelectedRound].Categories.RemoveAt(SelectedCategory),
            IsCategorySelected
        );

        public ICommand RemoveQuestionCommand => new RelayCommand(
            () => Quiz.Rounds[SelectedRound].Categories[SelectedCategory].Questions.RemoveAt(SelectedQuestion),
            IsQuestionSelected
        );

        public ICommand SetContentCommand => new RelayCommand(
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

        public ICommand RunValidationsCommand => new RelayCommand(
            () =>
            {
                ValidationResult = QuizPacker.Validate(Quiz);
                OnPropertyChanged(nameof(ValidationResult));
            },
            null
        );

        public ICommand NewQuizCommand => new RelayCommand(
            () =>
            {
                ValidationResult.FieldValidationResults.Clear();
                OnPropertyChanged(nameof(ValidationResult));
                Quiz = new(new ObservableCollection<QuizRound>());
                OnPropertyChanged(nameof(Quiz));
            },
            null
        );

        public ICommand SaveQuizCommand => new RelayCommand(
            () =>
            {
                SaveFileDialog dlg = new()
                {
                    Filter = "Question pack (*.qpck)|*.qpck",
                    DefaultExt = "qpck",
                    AddExtension = true
                };
                if (dlg.ShowDialog() == true)
                {
                    ValidationResult = QuizPacker.Pack(Quiz);
                    if (!ValidationResult.HasErrors())
                    {
                        BinarySerializer.SerializeToFile(Quiz, dlg.FileName);
                    }
                }
            },
            null
        );

        public ICommand OpenQuizCommand => new RelayCommand(
            () =>
            {
                OpenFileDialog openFileDialog = new()
                {
                    Filter = "Question pack (*.qpck)|*.qpck"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    Quiz = BinarySerializer.DeserializeFromFile<Quiz>(openFileDialog.FileName);
                    QuizPacker.Unpack(Quiz);
                    OnPropertyChanged(nameof(Quiz));
                }
            },
            null
        );

        public int SelectedRound { get; set; } = -1;
        public int SelectedCategory { get; set; } = -1;
        public int SelectedQuestion { get; set; } = -1;

        private bool IsRoundSelected() => SelectedRound >= 0 &&
                   SelectedRound < Quiz.Rounds.Count;

        private bool IsCategorySelected() => IsRoundSelected() &&
                   SelectedCategory >= 0 &&
                   SelectedCategory < Quiz.Rounds[SelectedRound].Categories.Count;

        private bool IsQuestionSelected() => IsCategorySelected() &&
                   SelectedQuestion >= 0 &&
                   SelectedQuestion < Quiz.Rounds[SelectedRound].Categories[SelectedCategory].Questions.Count;
    }
}
