using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zion
{
    public readonly ref struct ArenaSpan<T>
    {
        #region Data
        private readonly ArenaRegion<T> Region;

        public int Length => Region.Length;

        public ArenaChunkEnumerator<T> Chunks => Region.EnumerateChunks();
        #endregion

        #region Constructors
        internal ArenaSpan(ArenaRegion<T> Region)
        {
            this.Region = Region;
        }
        #endregion

        #region Indexers
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
        #endregion

        #region Operators
        public static bool operator ==(ArenaSpan<T> A, ArenaSpan<T> B)
        {
            return A.Region == B.Region;
        }

        public static bool operator !=(ArenaSpan<T> A, ArenaSpan<T> B)
        {
            return A.Region != B.Region;
        }
        #endregion

        #region PublicMethods
        public ArenaSpan<T> Slice(int Start)
        {
            return new ArenaSpan<T>(Region.Slice(Start));
        }

        public ArenaSpan<T> Slice(int Start, int Count)
        {
            return new ArenaSpan<T>(Region.Slice(Start, Count));
        }


        public void Sort(IComparer<T>? Comparer = null)
        {
            Comparer ??= Comparer<T>.Default;

            var Region = this.Region;
            var ChunkCount = Region.GetChunkCount();

            switch (ChunkCount)
            {
                case 0:
                    break;

                case 1:
                    Region.GetChunk(0).Sort(Comparer);
                    break;
                
                default:
                    //Optimize
                    var Buffer = ArrayPool<T>.Shared.Rent(Region.Length).AsSpan(0, Region.Length);
                    CopyTo(Buffer);
                    Buffer.Sort(Comparer);
                    Buffer.CopyTo(this);
                    break;
            }
        }

        public void Reverse()
        {
            var Region = this.Region;
            var ChunkCount = Region.GetChunkCount();

            if (ChunkCount <= 0)
            {
                return;
            }

            if (ChunkCount == 1)
            {
                Region.GetChunk(0).Reverse();
                return;
            }

            var FirstChunk = Arena<T>.GetChunkIndex(Region.Start);
            var LastChunk = Arena<T>.GetChunkIndex(Region.Start + Region.Length - 1);

            var LeftChunkIndex = FirstChunk;
            var RightChunkIndex = LastChunk;

            var LeftChunkOffset = Arena<T>.GetLocalIndex(Region.Start);
            var RightChunkEnd = Arena<T>.GetLocalIndex(Region.Start + Region.Length - 1) + 1;

            while (LeftChunkIndex < RightChunkIndex)
            {
                var LeftSpan = Region.Chunks[LeftChunkIndex].AsSpan(LeftChunkOffset);
                var RightSpan = Region.Chunks[RightChunkIndex].AsSpan(0, RightChunkEnd);

                var Min = Math.Min(LeftSpan.Length, RightSpan.Length);

                var LeftPart = LeftSpan.Slice(0, Min);
                var RightPart = RightSpan.Slice(RightSpan.Length - Min, Min);

                for (var i = 0; i < Min; i++)
                {
                    var j = Min - 1 - i;
                    (LeftPart[i], RightPart[j]) = (RightPart[j], LeftPart[i]);
                }

                if (LeftSpan.Length == Min)
                {
                    LeftChunkIndex++;
                    LeftChunkOffset = 0;
                }
                else
                {
                    LeftChunkOffset += Min;
                }

                if (RightSpan.Length == Min)
                {
                    RightChunkIndex--;
                    RightChunkEnd = Arena<T>.ChunkSize;
                }
                else
                {
                    RightChunkEnd -= Min;
                }
            }

            if (LeftChunkIndex == RightChunkIndex)
            {
                var Middle = Region.Chunks[LeftChunkIndex].AsSpan(LeftChunkOffset, RightChunkEnd - LeftChunkOffset);
                Middle.Reverse();
            }
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            Comparer ??= EqualityComparer<T>.Default;

            var Offset = 0;

            foreach (var Chunk in Region.EnumerateChunks())
            {
                var Index = Chunk.IndexOf(Item, Comparer);
                if (Index >= 0)
                {
                    return Offset + Index;
                }
                Offset += Chunk.Length;
            }

            return -1;
        }

        public int LastIndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            Comparer ??= EqualityComparer<T>.Default;

            var Offset = Length;

            foreach (var Chunk in Region.EnumerateChunks())
            {
                Offset -= Chunk.Length;

                var Index = Chunk.LastIndexOf(Item, Comparer);
                if (Index >= 0)
                {
                    return Offset + Index;
                }
            }

            return -1;
        }

        public int BinarySearch(T Item, IComparer<T>? Comparer = null)
        {
            Comparer ??= Comparer<T>.Default;

            var Low = 0;
            var High = Length - 1;

            while (Low <= High)
            {
                var Mid = Low + ((High - Low) >> 1);
                var Cmp = Comparer.Compare(Region[Mid], Item);

                if (Cmp == 0)
                {
                    return Mid;
                }
                if (Cmp < 0)
                {
                    Low = Mid + 1;
                }
                else
                {
                    High = Mid - 1;
                }
            }

            return ~Low;
        }


        public int FindIndex(Predicate<T> Match)
        {
            var Offset = 0;

            foreach (var Chunk in Region.EnumerateChunks())
            {
                for (var i = 0; i < Chunk.Length; i++)
                {
                    if (Match(Chunk[i]))
                    {
                        return Offset + i;
                    }
                }
                Offset += Chunk.Length;
            }

            return -1;
        }

        public int FindLastIndex(Predicate<T> Match)
        {
            var Offset = Length;

            foreach (var Chunk in Region.EnumerateChunks())
            {
                Offset -= Chunk.Length;

                for (var i = Chunk.Length - 1; i >= 0; i--)
                {
                    if (Match(Chunk[i]))
                    {
                        return Offset + i;
                    }
                }
            }

            return -1;
        }


        public T? Find(Predicate<T> Match)
        {
            var Index = FindIndex(Match);
            return Index == -1
                ? default
                : Region[Index];
        }

        public T? FindLast(Predicate<T> Match)
        {
            var Index = FindLastIndex(Match);
            return Index == -1
                ? default
                : Region[Index];
        }

        public List<T> FindAll(Predicate<T> Match)
        {
            var Results = new List<T>();

            foreach (var Chunk in Region.EnumerateChunks())
            {
                foreach (var Item in Chunk)
                {
                    if (Match(Item))
                    {
                        Results.Add(Item);
                    }
                }
            }

            return Results;
        }


        public bool Exists(Predicate<T> Match)
        {
            foreach (var Chunk in Region.EnumerateChunks())
            {
                foreach (var Item in Chunk)
                {
                    if (Match(Item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            Comparer ??= EqualityComparer<T>.Default;

            foreach (var Chunk in Region.EnumerateChunks())
            {
                if (Chunk.Contains(Item, Comparer))
                {
                    return true;
                }
            }

            return false;
        }


        public void ForEach(Action<T> Action)
        {
            foreach (var Chunk in Region.EnumerateChunks())
            {
                foreach (var Item in Chunk)
                {
                    Action(Item);
                }
            }
        }

        public bool TrueForAll(Predicate<T> Match)
        {
            foreach (var Chunk in Region.EnumerateChunks())
            {
                foreach (var Item in Chunk)
                {
                    if (!Match(Item))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public void CopyTo(Span<T> Destination)
        {
            if (Destination.Length < Length)
            {
                throw new ArgumentException
                (
                    $"The destination Span is shortest (Source: {Length}, Destination: {Destination.Length}"
                );
            }

            var Index = 0;

            foreach (var Chunk in Region.EnumerateChunks())
            {
                Chunk.CopyTo(Destination.Slice(Index));
                Index += Chunk.Length;
            }
        }

        public void CopyTo(ArenaSpan<T> Destination)
        {
            if (this == Destination)
            {
                return;
            }

            var SourceRegion = Region;
            var DestRegion = Destination.Region;

            if (SourceRegion.Length > DestRegion.Length)
            {
                throw new ArgumentException("Destination is too small.", nameof(Destination));
            }

            if (SourceRegion.Length == 0)
            {
                return;
            }

            if (!ReferenceEquals(SourceRegion.Chunks[0], DestRegion.Chunks[0]))
            {
                CopyForward(SourceRegion, DestRegion);
                return;
            }

            if (!ArenaRegion<T>.Overlaps(SourceRegion, DestRegion))
            {
                CopyForward(SourceRegion, DestRegion);
                return;
            }

            if (DestRegion.Start < SourceRegion.Start)
            {
                CopyForward(SourceRegion, DestRegion);
            }
            else
            {
                CopyBackward(SourceRegion, DestRegion);
            }
        }

        private static void CopyForward(ArenaRegion<T> Source, ArenaRegion<T> Destination)
        {
            var SourceChunkIndex = Arena<T>.GetChunkIndex(Source.Start);
            var DestChunkIndex = Arena<T>.GetChunkIndex(Destination.Start);

            var SourceChunkOffset = Arena<T>.GetLocalIndex(Source.Start);
            var DestChunkOffset = Arena<T>.GetLocalIndex(Destination.Start);

            var Remaining = Source.Length;

            while (Remaining > 0)
            {
                var SourceSpan = Source.Chunks[SourceChunkIndex].AsSpan(SourceChunkOffset);
                var DestSpan = Destination.Chunks[DestChunkIndex].AsSpan(DestChunkOffset);

                var Min = Math.Min(SourceSpan.Length, Remaining);

                SourceSpan.Slice(0, Min).CopyTo(DestSpan.Slice(0, Min));

                Remaining -= Min;

                if (SourceSpan.Length == Min)
                {
                    SourceChunkIndex++;
                    SourceChunkOffset = 0;
                }
                else
                {
                    SourceChunkOffset += Min;
                }

                if (DestSpan.Length == Min)
                {
                    DestChunkIndex++;
                    DestChunkOffset = 0;
                }
                else
                {
                    DestChunkOffset += Min;
                }
            }
        }

        private static void CopyBackward(ArenaRegion<T> Source, ArenaRegion<T> Destination)
        {
            var SourceChunkIndex = Arena<T>.GetChunkIndex(Source.Start + Source.Length - 1);
            var DestChunkIndex = Arena<T>.GetChunkIndex(Destination.Start + Destination.Length - 1);

            var SourceChunkEnd = Arena<T>.GetLocalIndex(Source.Start + Source.Length - 1) + 1;
            var DestChunkEnd = Arena<T>.GetLocalIndex(Destination.Start + Destination.Length - 1) + 1;

            var Remaining = Source.Length;

            while (Remaining > 0)
            {
                var SourceSpan = Source.Chunks[SourceChunkIndex].AsSpan(0, SourceChunkEnd);
                var DestSpan = Destination.Chunks[DestChunkIndex].AsSpan(0, DestChunkEnd);

                var Min = Math.Min(SourceSpan.Length, Remaining);

                SourceSpan.Slice(SourceSpan.Length - Min, Min)
                          .CopyTo(DestSpan.Slice(DestSpan.Length - Min, Min));

                Remaining -= Min;

                if (SourceSpan.Length == Min)
                {
                    SourceChunkIndex--;
                    SourceChunkEnd = Arena<T>.ChunkSize;
                }
                else
                {
                    SourceChunkEnd -= Min;
                }

                if (DestSpan.Length == Min)
                {
                    DestChunkIndex--;
                    DestChunkEnd = Arena<T>.ChunkSize;
                }
                else
                {
                    DestChunkEnd -= Min;
                }
            }
        }


        public T[] ToArray()
        {
            var Array = new T[Length];
            CopyTo(Array);
            return Array;
        }

        public List<T> ToList()
        {
            var List = new List<T>(Length);
            var Span = CollectionsMarshal.AsSpan(List);
            CollectionsMarshal.SetCount(List, Length);
            CopyTo(Span);
            return List;
        }


        public ArenaItemEnumerator<T> GetEnumerator()
        {
            return new ArenaItemEnumerator<T>(Region);
        }
        #endregion

        #region InternalMethods
        internal void CopyFrom(Span<T> Source)
        {
            if (Source.Length > Length)
            {
                throw new ArgumentException("Source is too large", nameof(Source));
            }

            int Index = 0;
            
            foreach (var Chunk in Chunks)
            {
                Source.Slice(Index, Chunk.Length).CopyTo(Chunk);
                Index += Chunk.Length;
            }
        }
        #endregion
    }
}