namespace Jeopardy.Core.Data.Validation
{
    public class FieldValidationResult
    {
        public FieldValidationResultType ValidationResultType { get; }
        public string Message { get; }
        public string? FieldName { get; }
        public string? FieldValue { get; }

        private FieldValidationResult(FieldValidationResultType validationResultType, string message, string fieldName = "", string fieldValue = "") => (ValidationResultType, Message, FieldValue, FieldValue) = (validationResultType, message, fieldName, fieldValue);

        public static FieldValidationResult Error(string message, string fieldName = "", string fieldValue = "") => new(FieldValidationResultType.Error, message, fieldName, fieldValue);

        public static FieldValidationResult Warning(string message, string fieldName = "", string fieldValue = "") => new(FieldValidationResultType.Warning, message, fieldName, fieldValue);

        public static FieldValidationResult Info(string message, string fieldName = "", string fieldValue = "") => new(FieldValidationResultType.Info, message, fieldName, fieldValue);

        public override string ToString()
        {
            var fieldName = string.IsNullOrWhiteSpace(FieldName) ? "" : $", \"{FieldName}\"";
            var fieldValue = string.IsNullOrWhiteSpace(FieldValue) ? "" : $": {FieldValue}";
            return $"{ValidationResultType} - {Message}{fieldName }{fieldValue}";
        }
    }
}
