namespace Zion
{
    public ref struct ArenaItemEnumerator<T>
    {
        private readonly Span<T[]> Chunks;
        private readonly int       Limit;
        private          int       CurrentIndex;

        public T Current { get; private set; }


        public ArenaItemEnumerator(ArenaRegion<T> Region)
        {
            Chunks = Region.Chunks;
            Limit = Region.Start + Region.Length;
            CurrentIndex = Region.Start - 1;
            Current = default!;
        }

        public ArenaItemEnumerator<T> GetEnumerator()
        {
            return this;
        }


        public bool MoveNext()
        {
            var Index = CurrentIndex + 1;
            if (Index >= Limit)
            {
                return false;
            }

            CurrentIndex = Index;
            Current = Chunks[Arena<T>.GetChunkIndex(Index)][Arena<T>.GetLocalIndex(Index)];
            return true;
        }
    }
}