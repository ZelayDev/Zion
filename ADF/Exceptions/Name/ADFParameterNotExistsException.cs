namespace Zion.ADF
{
    public sealed class ADFParameterNotExistsException : ADFException
    {
        public readonly string? ParameterName;

        public ADFParameterNotExistsException(string? ParameterName)
            : base($"Parameter '{ParameterName ?? "null"}' not exists.")
        {
            this.ParameterName = ParameterName;
        }
    }
}