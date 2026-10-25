using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zion
{
    public readonly struct ArenaRegion<T>
    {
        public readonly T[][] Chunks;
        public readonly int   Start;
        public readonly int   Length;

        public Segment Area => new Segment(Start, Length);
        
        
        public ArenaRegion(T[][] Chunks, Segment Area)
            : this(Chunks, Area.Start, Area.Length) { }
        
        public ArenaRegion(T[][] Chunks, int Start, int Length)
        {
            this.Chunks = Chunks;
            this.Start  = Start;
            this.Length = Length;
        }

        
        public T this[int Index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Length);

                var Global = Start + Index;
                var ChunkIndex = Arena<T>.GetChunkIndex(Global);
                var LocalIndex = Arena<T>.GetLocalIndex(Global);

                ref var FirstChunk = ref MemoryMarshal.GetArrayDataReference(Chunks);
                ref var Chunk = ref Unsafe.Add(ref FirstChunk, ChunkIndex);
                ref var FirstItem = ref MemoryMarshal.GetArrayDataReference(Chunk);

                return Unsafe.Add(ref FirstItem, LocalIndex);
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Length);

                var Global = Start + Index;
                var ChunkIndex = Arena<T>.GetChunkIndex(Global);
                var LocalIndex = Arena<T>.GetLocalIndex(Global);

                ref var FirstChunk = ref MemoryMarshal.GetArrayDataReference(Chunks);
                ref var Chunk = ref Unsafe.Add(ref FirstChunk, ChunkIndex);
                ref var FirstItem = ref MemoryMarshal.GetArrayDataReference(Chunk);

                Unsafe.Add(ref FirstItem, LocalIndex) = value;
            }
        }

        public T this[Index Index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this[Index.GetOffset(Length)];
            [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this[Index.GetOffset(Length)] = value;
        }

        
        public ArenaSpan<T> AsSpan()
        {
            return new ArenaSpan<T>(this);
        }
    }
}