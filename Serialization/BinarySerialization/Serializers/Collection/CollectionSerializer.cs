namespace Zion.Serialization
{
    public readonly struct CollectionSerializer<TCollection, T> : IBinarySerializer<TCollection>
        where TCollection : ICollection<T>
    {
        private readonly Action<BinaryWriter, T> ItemWriter;
        private readonly Func<BinaryReader, T>   ItemReader;
        private readonly Func<TCollection>       Factory;

        public CollectionSerializer(Action<BinaryWriter, T> Writer, Func<BinaryReader, T> Reader,
            Func<TCollection>                               Factory)
        {
            ItemWriter = Writer.NotNull();
            ItemReader = Reader.NotNull();
            this.Factory = Factory.NotNull();
        }

        public void Write(BinaryWriter Writer, TCollection Collection)
        {
            var ItemWriter = this.ItemWriter;

            Writer.Write(Collection.Count);
            foreach (var Item in Collection)
            {
                ItemWriter(Writer, Item);
            }
        }

        public TCollection Read(BinaryReader Reader)
        {
            var ItemReader = this.ItemReader;
            var Result = Factory();

            var Count = Reader.ReadInt32();
            for (var i = 0; i < Count; i++)
            {
                Result.Add(ItemReader(Reader));
            }

            return Result;
        }
    }
}