using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zion
{
    public readonly struct ArenaRegion<T> : IEquatable<ArenaRegion<T>>
    {
        #region Data
        public readonly T[][] Chunks;
        public readonly int   Start;
        public readonly int   Length;
        #endregion

        #region Properties
        public Segment Area => new(Start, Length);
        #endregion

        #region Constructors
        public ArenaRegion(T[][] Chunks, Segment Area)
            : this(Chunks, Area.Start, Area.Length) { }

        public ArenaRegion(T[][] Chunks, int Start, int Length)
        {
            this.Chunks = Chunks;
            this.Start = Start;
            this.Length = Length;
        }
        #endregion

        #region Indexers
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this[Index.GetOffset(Length)];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => this[Index.GetOffset(Length)] = value;
        }
        #endregion
        
        #region Operators
        public static bool operator ==(ArenaRegion<T> A, ArenaRegion<T> B)
        {
            ArgumentNullException.ThrowIfNull(A.Chunks);
            ArgumentNullException.ThrowIfNull(B.Chunks);
            return ReferenceEquals(A.Chunks[0], B.Chunks[0])
                && A.Start == B.Start
                && A.Length == B.Length;
        }

        public static bool operator !=(ArenaRegion<T> A, ArenaRegion<T> B)
        {
            ArgumentNullException.ThrowIfNull(A.Chunks);
            ArgumentNullException.ThrowIfNull(B.Chunks);
            return !ReferenceEquals(A.Chunks[0], B.Chunks[0])
                || A.Start != B.Start
                || A.Length != B.Length;
        }
        #endregion

        #region OverrideMethods
        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is ArenaRegion<T> Region && this == Region;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Chunks, Start, Length);
        }
        
        public bool Equals(ArenaRegion<T> other)
        {
            return Chunks.Equals(other.Chunks) && Start == other.Start && Length == other.Length;
        }
        #endregion
        

        #region PublicMethods
        public static bool Overlaps(ArenaRegion<T> A, ArenaRegion<T> B)
        {
            ArgumentNullException.ThrowIfNull(A.Chunks);
            ArgumentNullException.ThrowIfNull(B.Chunks);

            if (A.Length == 0 || B.Length == 0)
            {
                return false;
            }

            return ReferenceEquals(A.Chunks[0], B.Chunks[0])
                && A.Start < B.Start + B.Length
                && B.Start < A.Start + A.Length;
        }
        
        
        public int GetChunkCount()
        {
            if (Length == 0)
            {
                return 0;
            }

            var FirstChunk = Arena<T>.GetChunkIndex(Start);
            var LastChunk = Arena<T>.GetChunkIndex(Start + Length - 1);

            return LastChunk - FirstChunk + 1;
        }

        public Span<T> GetChunk(int ChunkIndex)
        {
            var FirstChunk = Arena<T>.GetChunkIndex(Start);
            var LastChunk = Arena<T>.GetChunkIndex(Start + Length - 1);

            if ((uint)ChunkIndex >= (uint)Chunks.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(ChunkIndex));
            }
            if (ChunkIndex < FirstChunk || ChunkIndex > LastChunk)
            {
                throw new ArgumentOutOfRangeException(nameof(ChunkIndex), "Chunk is not part of this region.");
            }

            var ChunkStart = ChunkIndex == FirstChunk ? Arena<T>.GetLocalIndex(Start) : 0;
            var ChunkEnd = ChunkIndex == LastChunk
                ? Arena<T>.GetLocalIndex(Start + Length - 1) + 1
                : Arena<T>.ChunkSize;

            return Chunks[ChunkIndex].AsSpan(ChunkStart, ChunkEnd - ChunkStart);
        }


        public ArenaRegion<T> Slice(int Start)
        {
            if (Start < 0 || Start >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Start));
            }
            if (Start == 0)
            {
                return this;
            }

            return new ArenaRegion<T>(Chunks, Start, Length - Start);
        }

        public ArenaRegion<T> Slice(int Start, int Count)
        {
            if (Start < 0 || Start >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Start));
            }
            if (Count < 0 || Start + Count > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Length));
            }

            return new ArenaRegion<T>(Chunks, Start, Length - Start);
        }


        public ArenaSpan<T> AsSpan()
        {
            return new ArenaSpan<T>(this);
        }


        public ArenaChunkEnumerator<T> EnumerateChunks()
        {
            return new ArenaChunkEnumerator<T>(this);
        }
        #endregion
    }
}