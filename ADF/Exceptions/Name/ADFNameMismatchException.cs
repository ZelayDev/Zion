namespace Zion.ADF
{
    public sealed class ADFNameMismatchException : ADFMismatchException
    {
        public readonly string? Value, Target;

        public ADFNameMismatchException(string? Name, string? TargetName)
            : base($"Name mismatch: Name: '{Name ?? "null"}', Target: '{TargetName ?? "null"}'")
        {
            Value = Name;
            Target = TargetName;
        }
    }
}