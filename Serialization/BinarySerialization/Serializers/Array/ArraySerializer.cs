namespace Zion.Serialization
{
    public readonly struct ArraySerializer<T> : IBinarySerializer<T[]>
    {
        private readonly Action<BinaryWriter, T> ItemWriter;
        private readonly Func<BinaryReader, T>   ItemReader;

        public ArraySerializer(Action<BinaryWriter, T> Writer, Func<BinaryReader, T> Reader)
        {
            ItemWriter = Writer.NotNull();
            ItemReader = Reader.NotNull();
        }


        public void Write(BinaryWriter Writer, T[] Array)
        {
            var ItemWriter = this.ItemWriter;

            Writer.Write(Array.Length);
            foreach (var Item in Array)
            {
                ItemWriter(Writer, Item);
            }
        }

        public T[] Read(BinaryReader Reader)
        {
            var ItemReader = this.ItemReader;

            var Count = Reader.ReadInt32();
            var Result = new T[Count];

            for (var i = 0; i < Count; i++)
            {
                Result[i] = ItemReader(Reader);
            }

            return Result;
        }
    }
}