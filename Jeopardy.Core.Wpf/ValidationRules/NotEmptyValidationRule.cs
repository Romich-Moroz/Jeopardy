using System.Globalization;
using System.Windows.Controls;

namespace Jeopardy.Core.Wpf.ValidationRules
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public string Message { get; set; } = "Field is required.";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo) => string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, Message)
                : ValidationResult.ValidResult;
    }
}
