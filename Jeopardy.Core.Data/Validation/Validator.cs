namespace Jeopardy.Core.Data.Validation
{
    public static class Validator
    {
        public static bool IsNullOrEmpty<T>(T field) => field is string s ? string.IsNullOrEmpty(s) : field == null;

        public static bool InRange<T>(T value, T min, T max) where T : IComparable => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }
}
