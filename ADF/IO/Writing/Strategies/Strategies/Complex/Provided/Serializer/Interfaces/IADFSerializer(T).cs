namespace Zion.ADF
{
    public interface IADFSerializer<T> : IADFSerializer
    {
        Type IADFSerializer.TargetType => typeof(T);

        void IADFSerializer.Write(ADFObjectWriter Writer, object Value)
        {
            if (Value is T Typed)
            {
                Write(Writer, Typed);
            }
            else
            {
                throw new InvalidCastException($"Value is not {TargetType}");
            }
        }

        public void Write(ADFObjectWriter Writer, T Value);
    }
}