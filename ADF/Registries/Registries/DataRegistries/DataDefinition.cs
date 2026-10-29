namespace Zion.ADF
{
    internal readonly struct DataDefinition
    {
        public readonly uint FormatId;
        public readonly uint Page;
        public readonly long  Position;

        public DataDefinition(uint FormatId, uint Page, long Position)
        {
            this.FormatId = FormatId;
            this.Page     = Page;
            this.Position = Position;
        }
    }
}