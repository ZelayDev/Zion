namespace Zion
{
    public ref struct ArenaChunkEnumerator<T>
    {
        private readonly T[][] Chunks;
        private readonly int   Start;
        private readonly int   FirstChunk;

        private int Index;
        private int Remaining;

        public Span<T> Current { get; private set; }

        
        public ArenaChunkEnumerator(ArenaRegion<T> Region)
        {
            Chunks = Region.Chunks;
            Start = Region.Start;
            FirstChunk = Arena<T>.GetChunkIndex(Region.Start);
            Index = -1;
            Remaining = Region.Length;
            Current = default;
        }

        public ArenaChunkEnumerator<T> GetEnumerator()
        {
            return this;
        }

        
        public bool MoveNext()
        {
            if (Remaining == 0)
            {
                return false;
            }

            Index++;

            var ChunkIndex = FirstChunk + Index;
            var ChunkStart = Index == 0 ? Arena<T>.GetLocalIndex(Start) : 0;
            var Count = Math.Min(Arena<T>.ChunkSize - ChunkStart, Remaining);

            Current = Chunks[ChunkIndex].AsSpan(ChunkStart, Count);
            Remaining -= Count;
            return true;
        }
    }
}