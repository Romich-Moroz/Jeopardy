using System.Collections.ObjectModel;

namespace Jeopardy.Core.Data.Validation
{
    public class ValidationResult
    {
        public ObservableCollection<FieldValidationResult> FieldValidationResults { get; private set; } = new ObservableCollection<FieldValidationResult>();

        public bool HasErrors()
        {
            return FieldValidationResults.Any(r => r.ValidationResultType == FieldValidationResultType.Error);
        }

        public bool HasWarnings()
        {
            return FieldValidationResults.Any(r => r.ValidationResultType == FieldValidationResultType.Warning);
        }

        public bool HasInfo()
        {
            return FieldValidationResults.Any(r => r.ValidationResultType == FieldValidationResultType.Info);
        }
    }
}
