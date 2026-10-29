namespace Zion.ADF
{
    public sealed class ADFFormatMismatchException : ADFMismatchException
    {
        public readonly uint Value, Target;

        public ADFFormatMismatchException(uint Value, uint Target)
            : base($"Format mismatch: Value: '{Value}', Target: {Target}")
        {
            this.Value = Value;
            this.Target = Target;
        }
    }
}