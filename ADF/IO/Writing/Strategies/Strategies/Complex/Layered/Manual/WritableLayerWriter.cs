namespace Zion.ADF
{
    internal sealed class WritableLayerWriter<T> : IADFSerializer<T>
    {
        private readonly Action<ADFObjectWriter, T> WriteAction;

        public WritableLayerWriter(Action<ADFObjectWriter, T> WriteAction)
        {
            this.WriteAction = WriteAction;
        }

        public void Write(ADFObjectWriter Writer, T Value)
        {
            WriteAction(Writer, Value);
        }
    }
}