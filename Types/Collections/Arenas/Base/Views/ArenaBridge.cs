using System.Runtime.CompilerServices;

namespace Zion
{
    internal readonly struct ArenaBridge<T>
    {
        private readonly Arena<T>       Source;
        private readonly ArenaRegion<T> Region;


        public bool IsDisposed => Source is null;

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Region.Length;
        }


        internal ArenaBridge(Arena<T> Source, ArenaRegion<T> Region)
        {
            this.Source = Source;
            this.Region = Region;
        }


        public T this[int Index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Region[Index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Region[Index] = value;
        }

        public T this[Index Index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Region[Index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Region[Index] = value;
        }


        public bool IsFrom(Arena<T> Arena)
        {
            return ReferenceEquals(Arena, Source);
        }

        public void Dispose(ArenaCollection<T> Collection)
        {
            Source.Release(Collection);
        }

        public ArenaSpan<T> AsSpan()
        {
            return Region.AsSpan();
        }
        
        public A Allocate<A>(int Capacity) where A : ArenaCollection<T>, new()
        {
            return Source.Allocate<A>(Capacity);
        }
        

        internal void Clear()
        {
            Region.Clear();
        }
        
        internal void Clear(int Length)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(Length, Region.Length);
            new ArenaSpan<T>(new ArenaRegion<T>(Region.Chunks, Region.Start, Length)).Clear();
        }

        internal ArenaRegion<T> GetRegion()
        {
            return Region;
        }

        internal Segment GetArea()
        {
            return Region.Area;
        }


        internal static void Expand(ArenaCollection<T> Collection, int NewCapacity, ref ArenaBridge<T> Bridge)
        {
            if (NewCapacity > Bridge.Length)
            {
                Bridge = Bridge.Source.Expand(Collection, NewCapacity);
            }
        }

        internal static void Trim(ArenaCollection<T> Collection, int Length, ref ArenaBridge<T> Bridge)
        {
            if (Length < Bridge.Length)
            {
                Bridge = Bridge.Source.Trim(Collection, Length);
            }
        }
    }
}