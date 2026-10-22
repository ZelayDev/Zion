namespace Zion
{
    public readonly struct ArenaSpan<T>
    {
        public readonly Arena<T> Source;
        public readonly int Start;
        public readonly int Length;

        public ArenaSpan(Arena<T> Source, int Start, int Length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Start);
            ArgumentOutOfRangeException.ThrowIfNegative(Length);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(Length, Source.Capacity);
            
            this.Source = Source.NotNull();
            this.Start  = Start;
            this.Length = Length;
        }
    }
}