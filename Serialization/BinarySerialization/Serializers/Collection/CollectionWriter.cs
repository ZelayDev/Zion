using Zion.Serialization;

namespace Zion
{
    public readonly struct CollectionWriter<T> : IBinaryWriter<ICollection<T>>
    {
        private readonly Action<BinaryWriter, T> ItemWriter;

        public CollectionWriter(Action<BinaryWriter, T> Writer)
        {
            ItemWriter = Writer.NotNull();
        }

        public void Write(BinaryWriter Writer, ICollection<T> Collection)
        {
            var ItemWriter = this.ItemWriter;

            Writer.Write(Collection.Count);
            Collection.ForEach(Item => ItemWriter(Writer, Item));
        }
    }
}