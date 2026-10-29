namespace Zion.ADF
{
    internal readonly struct FormatCorrection
    {
        public readonly int Index;
        public readonly Parameter[] NewParameters;

        public FormatCorrection(int Index, Parameter[] NewParameters)
        {
            this.Index = Index;
            this.NewParameters = NewParameters.NotNull();
        }
    }
}