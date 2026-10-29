namespace Zion
{
    public ref struct ArenaReadOnlyChunkEnumerator<T>
    {
        private ArenaChunkEnumerator<T> Inner;

        public ReadOnlySpan<T> Current { get; private set; }


        internal ArenaReadOnlyChunkEnumerator(ArenaRegion<T> Region)
        {
            Inner = new ArenaChunkEnumerator<T>(Region);
        }

        public ArenaReadOnlyChunkEnumerator<T> GetEnumerator()
        {
            return this;
        }


        public bool MoveNext()
        {
            if (Inner.MoveNext())
            {
                Current = Inner.Current;
                return true;
            }
            return false;
        }
    }
}