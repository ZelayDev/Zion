namespace Zion
{
    public static class StringFormatter
    {
        public static string ToString<T>(IEnumerable<T> Enumerable)
        {
            return $"[{String.Join(", ", Enumerable.Select(static Item => Item?.ToString() ?? "null"))}]";
        }

        public static string ToString(IEnumerable<string> Enumerable)
        {
            static string FormatString(string? String)
            {
                return String is null ? "null" : $"\"{String}\"";
            }

            return $"[{String.Join(", ", Enumerable.Select(FormatString))}]";
        }
    }
}