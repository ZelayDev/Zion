namespace Zion.Serialization
{
    public readonly struct ArrayReader<T> : IBinaryReader<T[]>
    {
        private readonly Func<BinaryReader, T> ItemReader;

        public ArrayReader(Func<BinaryReader, T> Reader)
        {
            this.ItemReader = Reader.NotNull();
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