namespace Zion.ADF
{
    public interface IWritableRegistry : IADFWritable
    {
        public bool IsChanged { get; }
    }
}