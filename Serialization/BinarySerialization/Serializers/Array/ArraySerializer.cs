namespace Zion.Serialization
{
    public readonly struct ArraySerializer<T> : IBinarySerializer<T[]>
    {
        private readonly Action<BinaryWriter, T> ItemWriter;
        private readonly Func<BinaryReader, T> ItemReader;

        public ArraySerializer(Action<BinaryWriter, T> Writer, Func<BinaryReader, T> Reader)
        {
            this.ItemWriter = Writer.NotNull();
            this.ItemReader = Reader.NotNull();
        }


        public void Write(BinaryWriter Writer, T[] Array)
        {
            var ItemWriter = this.ItemWriter;

            Writer.Write(Array.Length);
            foreach (T Item in Array)
            {
                ItemWriter(Writer, Item);
            }
        }

        public T[] Read(BinaryReader Reader)
        {
            var ItemReader = this.ItemReader;

            int Count = Reader.ReadInt32();
            T[] Result = new T[Count];

            for (int i = 0; i < Count; i++)
            {
                Result[i] = ItemReader(Reader);
            }

            return Result;
        }
    }
}