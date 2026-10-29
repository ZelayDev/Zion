namespace Zion.ADF
{
    public sealed class ADFMissingParametersException : ADFException
    {
        public readonly int Writed;
        public readonly int Count;

        public int Excepted => Count - Writed;

        public ADFMissingParametersException(int Writed, int Count)
            : base($"Missing parameters: {Writed} written, {Count - Writed} expected")
        {
            this.Writed = Writed;
            this.Count  = Count;
        }
    }
}