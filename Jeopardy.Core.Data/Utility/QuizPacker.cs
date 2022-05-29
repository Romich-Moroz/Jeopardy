using Jeopardy.Core.Data.Quiz;
using Jeopardy.Core.Data.Quiz.Constants;
using Jeopardy.Core.Data.Validation;
using System.Collections.ObjectModel;
using System.Text;

namespace Jeopardy.Core.Data.Utility
{
    public static class QuizPacker
    {
        public static ValidationResult Validate(Quiz.Quiz quiz)
        {
            ValidationResult result = new();

            for (var i = 0; i < quiz.Rounds.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(quiz.Rounds[i].Name))
                {
                    result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("Round name is empty", i)));
                }

                for (var j = 0; j < quiz.Rounds[i].Categories.Count; j++)
                {
                    if (string.IsNullOrWhiteSpace(quiz.Rounds[i].Categories[j].Name))
                    {
                        result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("Category name is empty", i, j)));
                    }

                    for (var k = 0; k < quiz.Rounds[i].Categories[j].Questions.Count; k++)
                    {
                        if (quiz.Rounds[i].Categories[j].Questions[k].Price <= 0)
                        {
                            result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("Price is invalid", i, j, k)));
                        }

                        if (string.IsNullOrWhiteSpace(quiz.Rounds[i].Categories[j].Questions[k].CorrectAnswer))
                        {
                            result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("Correct answer is not set", i, j, k)));
                        }

                        if (string.IsNullOrWhiteSpace(quiz.Rounds[i].Categories[j].Questions[k].TaskDescription))
                        {
                            result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("Task description is empty", i, j, k)));
                        }

                        if (quiz.Rounds[i].Categories[j].Questions[k].ContentType is < ContentType.Text or > ContentType.Video)
                        {
                            result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("Specified content type is not supported", i, j, k)));
                        }

                        if (string.IsNullOrWhiteSpace(quiz.Rounds[i].Categories[j].Questions[k].ContentPath))
                        {
                            result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("Content is empty", i, j, k)));
                        }
                        else if (quiz.Rounds[i].Categories[j].Questions[k].ContentType != ContentType.Text)
                        {
                            switch (quiz.Rounds[i].Categories[j].Questions[k].ContentAccessType)
                            {
                                case ContentAccessType.Embedded:
                                    if (!File.Exists(quiz.Rounds[i].Categories[j].Questions[k].ContentPath))
                                    {
                                        if (quiz.Rounds[i].Categories[j].Questions[k].RawContent.Length == 0)
                                        {
                                            result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("File does not exist", i, j, k)));
                                        }
                                        else
                                        {
                                            result.FieldValidationResults.Add(FieldValidationResult.Warning(CreateMessage("File does not exist, previous content remains", i, j, k)));
                                        }
                                    }

                                    break;
                                //case ContentAccessType.Link:
                                //    if (!IsUrlValid(quiz.Rounds[i].Categories[j].Questions[k].ContentPath))
                                //    {
                                //        result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("Url is not valid", i, j, k)));
                                //    }
                                //    break;
                                default:
                                    result.FieldValidationResults.Add(FieldValidationResult.Error(CreateMessage("Specified content access type is not supported", i, j, k)));
                                    break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static ValidationResult Pack(Quiz.Quiz quiz)
        {
            ValidationResult result = Validate(quiz);

            if (!result.HasErrors())
            {
                foreach (QuizRound? round in quiz.Rounds)
                {
                    foreach (QuizCategory? category in round.Categories)
                    {
                        foreach (Question? question in category.Questions)
                        {
                            switch (question.ContentType)
                            {
                                case ContentType.Text:
                                    PackTextQuestion(question);
                                    break;
                                case ContentType.Image:
                                case ContentType.Video:
                                case ContentType.Sound:
                                    if (question.ContentAccessType == ContentAccessType.Embedded)
                                    {
                                        PackMediaQuestion(question);
                                    }

                                    break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static void Unpack(Quiz.Quiz quiz)
        {
            foreach (QuizRound? round in quiz.Rounds)
            {
                foreach (QuizCategory? category in round.Categories)
                {
                    category.Questions = new ObservableCollection<Question>(category.Questions);
                }

                round.Categories = new ObservableCollection<QuizCategory>(round.Categories);
            }

            quiz.Rounds = new ObservableCollection<QuizRound>(quiz.Rounds);
        }

        private static void PackTextQuestion(Question question) => question.RawContent = Encoding.UTF8.GetBytes(question.ContentPath);

        private static void PackMediaQuestion(Question question)
        {
            if (File.Exists(question.ContentPath))
            {
                question.RawContent = File.ReadAllBytes(question.ContentPath);
            }
        }

        //private static bool IsUrlValid(string? url) => Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
        //        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        private static string CreateMessage(string message, int round, int category = -1, int question = -1)
        {
            var rnd = $"R{round}:";
            var cat = category == -1 ? "" : $"C{category}:";
            var qst = question == -1 ? "" : $"Q{question}:";

            return $"{rnd}{cat}{qst} {message}";
        }
    }
}
