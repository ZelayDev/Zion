using Zion.Serialization;

namespace Zion
{
    public readonly struct CollectionReader<TCollection, T> : IBinaryReader<TCollection> where TCollection : ICollection<T>
    {
        private readonly Func<BinaryReader, T> ItemReader;
        private readonly Func<int, TCollection> Factory;

        public CollectionReader(Func<BinaryReader, T> ItemReader, Func<int, TCollection> Factory)
        {
            this.ItemReader = ItemReader.NotNull();
            this.Factory = Factory.NotNull();
        }

        public TCollection Read(BinaryReader Reader)
        {
            int Count = Reader.ReadInt32();
            var ItemReader = this.ItemReader;
            TCollection Result = Factory(Count);

            for (int i = 0; i < Count; i++)
            {
                Result.Add(ItemReader(Reader));
            }

            return Result;
        }
    }
}