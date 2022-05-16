namespace Jeopardy.Core.Data.Validation
{
    public static class Validator
    {
        public static bool IsNullOrEmpty<T>(T field)
        {
            if (field is string s)
            {
                return string.IsNullOrEmpty(s);
            }
            else
            {
                return field == null;
            }
        }

        public static bool InRange<T>(T value, T min, T max) where T : IComparable
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                return false;
            }

            return true;
        }
    }
}
