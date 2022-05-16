using System.Collections.ObjectModel;

namespace Jeopardy.Core.Data.Validation
{
    public class ValidationResult
    {
        public ObservableCollection<FieldValidationResult> FieldValidationResults { get; private set; } = new ObservableCollection<FieldValidationResult>();

        public bool HasErrors() => FieldValidationResults.Any(r => r.ValidationResultType == FieldValidationResultType.Error);

        public bool HasWarnings() => FieldValidationResults.Any(r => r.ValidationResultType == FieldValidationResultType.Warning);

        public bool HasInfo() => FieldValidationResults.Any(r => r.ValidationResultType == FieldValidationResultType.Info);
    }
}
