namespace Zion.Serialization
{
    public readonly struct BinarySerializer<T> : IBinarySerializer<T>
    {
        private readonly Action<BinaryWriter, T> Writer;
        private readonly Func<BinaryReader, T> Reader;

        public BinarySerializer(Action<BinaryWriter, T> Writer, Func<BinaryReader, T> Reader)
        {
            this.Writer = Writer.NotNull();
            this.Reader = Reader.NotNull();
        }

        public void Write(BinaryWriter Writer, T Value)
        {
            this.Writer(Writer, Value);
        }
        
        public T Read(BinaryReader Reader)
        {
            return this.Reader(Reader);
        }
    }
}