namespace Zion.ADF
{
    public sealed class ADFFormatNotExistsException : ADFException
    {
        public readonly uint FormatId;

        public ADFFormatNotExistsException(uint FormatId)
            : base($"Format with id '{FormatId}' not exists")
        {
            this.FormatId = FormatId;
        }
    }
}