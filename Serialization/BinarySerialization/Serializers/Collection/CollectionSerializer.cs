namespace Zion.Serialization
{
    public readonly struct CollectionSerializer<TCollection, T> : IBinarySerializer<TCollection> where TCollection : ICollection<T>
    {
        private readonly Action<BinaryWriter, T> ItemWriter;
        private readonly Func<BinaryReader, T> ItemReader;
        private readonly Func<TCollection> Factory;

        public CollectionSerializer(Action<BinaryWriter, T> Writer, Func<BinaryReader, T> Reader, Func<TCollection> Factory)
        {
            this.ItemWriter = Writer.NotNull();
            this.ItemReader = Reader.NotNull();
            this.Factory = Factory.NotNull();
        }

        public void Write(BinaryWriter Writer, TCollection Collection)
        {
            var ItemWriter = this.ItemWriter;

            Writer.Write(Collection.Count);
            foreach (T Item in Collection)
            {
                ItemWriter(Writer, Item);
            }
        }

        public TCollection Read(BinaryReader Reader)
        {
            var ItemReader = this.ItemReader;
            TCollection Result = Factory();

            int Count = Reader.ReadInt32();
            for (int i = 0; i < Count; i++)
            {
                Result.Add(ItemReader(Reader));
            }

            return Result;
        }
    }
}