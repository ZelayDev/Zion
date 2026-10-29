namespace Zion.ADF
{
    public interface IADFSerializer : ILayerWriteInfo
    {
        public Type TargetType { get; }

        public void Write(ADFObjectWriter Writer, object Value);
    }
}