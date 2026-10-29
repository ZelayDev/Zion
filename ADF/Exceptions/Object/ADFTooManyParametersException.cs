namespace Zion.ADF
{
    public sealed class ADFTooManyParametersException : ADFMismatchException
    {
        public readonly int Count, Limit;

        public ADFTooManyParametersException(int Count, int Limit)
            : base($"Too many parameters: expected {Count}, but received {Limit}")
        {
            this.Count = Count;
            this.Limit = Limit;
        }
    }
}