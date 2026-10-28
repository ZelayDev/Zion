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


        internal void Clear()
        {
            Clear(Length);
        }
        
        internal void Clear(int Length)
        {
            if (Length == 0)
            {
                return;
            }

            var Chunks = Region.Chunks;
            var Remaining = Length;

            var FirstChunk = Arena<T>.GetChunkIndex(Region.Start);
            var LocalStart = Arena<T>.GetLocalIndex(Region.Start);

            for (var i = FirstChunk; i < Chunks.Length && Remaining > 0; i++)
            {
                var StartInChunk = i == FirstChunk ? LocalStart : 0;
                var Count = Math.Min(Arena<T>.ChunkSize - StartInChunk, Remaining);

                Array.Clear(Chunks[i], StartInChunk, Count);
                Remaining -= Count;
            }
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
            throw new NotImplementedException(); //TODO
        }
    }
}