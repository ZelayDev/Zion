namespace Zion.ADF
{
    public sealed class ADFObjectIsNullException : ADFException
    {
        public readonly string ParameterName;

        public ADFObjectIsNullException(string ParameterName)
            : base($"{ParameterName} cannot be null")
        {
            this.ParameterName = ParameterName;
        }
    }
}