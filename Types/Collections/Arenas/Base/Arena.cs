using System.Diagnostics.CodeAnalysis;

namespace Zion
{
    public sealed class Arena<T>
    {
        #region Constants
        internal const int BinaryGroupSize = 4;
        internal const int GroupSize       = 1 << BinaryGroupSize;

        internal const int BinaryBufferAccuracy = 9;
        internal const int BufferAccuracy       = 1 << BinaryBufferAccuracy;
        #endregion

        #region Data
        private readonly HashSet<ArenaCollection<T>> Collections;

        private T[]      Data;
        private BitArray BitMap;
        private int      Count;
        #endregion

        #region Properties
        public int Capacity
        {
            get => Data.Length;
            [MemberNotNull(nameof(Data), nameof(BitMap))]
            set
            {
                if (Data is null || value > Data.Length || BitMap is null)
                {
                    value = RoundToBufferSize(value);
                    BitMap = BitArray.Resize(BitMap, GetGroupCount(value));
                    Array.Resize(ref Data, value);
                }
            }
        }

        private int Used;
        #endregion

        #region Constructors
        public Arena() : this(1024) { }

        public Arena(int Capacity)
        {
            Collections = new HashSet<ArenaCollection<T>>(ReferenceEqualityComparer.Instance);
            this.Capacity = RoundToBufferSize(Math.Max(1024, Capacity));
        }
        #endregion
        
        #region Indexers
        internal T this[int Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }
        internal T this[Index Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }
        #endregion

        #region PublicMethods
        public ArenaArray<T> GetArray(int Size)
        {
            return Allocate<ArenaArray<T>>(Size, static Span => new ArenaArray<T>(Span));
        }

        public ArenaBuffer<T> GetBuffer(int Size)
        {
            return Allocate<ArenaBuffer<T>>(RoundToGroup(Size), static Span => new ArenaBuffer<T>(Span));
        }

        public ArenaList<T> GetList(int Size)
        {
            return Allocate<ArenaList<T>>(RoundToGroup(Size), static Span => new ArenaList<T>(Span));
        }

        public ArenaQueue<T> GetQueue(int Size)
        {
            return Allocate<ArenaQueue<T>>(RoundToGroup(Size), static Span => new ArenaQueue<T>(Span));
        }


        public A Allocate<A>(int Size, Func<ArenaSpan<T>, A> Fabric) where A : ArenaCollection<T>
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Size);

            var Start = GetFreeArea(Size);

            MarkArea(Start, Size, true);
            UpdateCount(Start + Size);

            var Span       = new ArenaSpan<T>(this, Start, Size);
            var Collection = Fabric(Span);

            Collections.Add(Collection);

            return Collection;
        }


        public T[] ToArray()
        {
            return ZArray.Clone(Data);
        }

        public T[] ToArray(int Start, int Length)
        {
            return ZArray.GetSubArray(Data, Start, Length);
        }


        public void DisposeAll()
        {
            foreach (var Collection in Collections) Collection.Dispose();

            Collections.Clear();

            BitMap.Fill(false);
            Array.Clear(Data, 0, Used);

            Count = 0;
            Used = 0;
        }
        #endregion

        #region InternalMethods
        internal Span<T> AsSpan(ArenaCollection<T> Collection)
        {
            CheckCollection(Collection);

            return Data.AsSpan(Collection.Start, Collection.Length);
        }

        internal Span<T> AsSpan(ArenaCollection<T> Collection, int Start, int Count)
        {
            CheckCollection(Collection);

            var ArenaLength = Collection.Length;

            ArgumentOutOfRangeException.ThrowIfNegative(Start);
            ArgumentOutOfRangeException.ThrowIfNegative(Count);
            ArgumentOutOfRangeException.ThrowIfBeyond(Start + Count, ArenaLength);

            return Data.AsSpan(Collection.Start + Start, Count);
        }

        internal Memory<T> AsMemory(ArenaCollection<T> Collection)
        {
            CheckCollection(Collection);

            return Data.AsMemory(Collection.Start, Collection.Length);
        }

        internal Segment Expand(ArenaCollection<T> Collection, int Size)
        {
            CheckCollection(Collection);

            if (TryExpand(Collection, Size, out var Expanded)) return Expanded;

            var Start = GetFreeArea(Size);

            MarkArea(Start, Size, true);
            UpdateCount(Start + Size);

            CopyTo(Collection, Start);
            MarkArea(Collection, false);

            return new Segment(Start, Size);
        }

        internal Segment TrimExcess(ArenaCollection<T> Collection, int Size)
        {
            CheckCollection(Collection);

            if (Size < GroupSize || Size >= Collection.Length) return new Segment(Collection.Start, Collection.Length);

            var Start = Collection.Start;

            if (Count == Start + Collection.Length) Count = Start + Size;

            MarkArea(Start, Size, false);

            return new Segment(Start, Size << BinaryGroupSize);
        }

        internal void Release(ArenaCollection<T> Collection)
        {
            CheckCollection(Collection);
            MarkArea(Collection.Start, Collection.Length, false);

            Collections.Remove(Collection);

            if (IsLastCollection(Collection)) Count = Collection.Start;

            Collection.Dispose();
        }
        #endregion

        #region PrivateMethods
        private void CheckCollection(ArenaCollection<T> Collection)
        {
            if (Collection.IsDisposed) throw new ObjectDisposedException(nameof(Collection));

            if (!ReferenceEquals(this, Collection.Source))
                throw new InvalidOperationException("Arena not contains this ArenaSpan");
        }

        private bool IsLastCollection(ArenaCollection<T> Collection)
        {
            return Collection.Start + Collection.Length == Count;
        }

        private void UpdateCount(int NewCount)
        {
            if (NewCount > Count) Count = RoundToGroup(NewCount);
        }

        private void UpdateUsed(int NewCount)
        {
            if (NewCount > Used) Used = NewCount;
        }

        private void MarkArea(ArenaCollection<T> Area, bool Busy)
        {
            MarkArea(Area.Start, Area.Length, Busy);
        }

        private void MarkArea(int Start, int Count, bool Busy)
        {
            Start = GetFullGroupCount(Start);
            Count = GetGroupCount(Count);

            BitMap.Fill(Start, Count, Busy);

            if (Busy) UpdateUsed((Start + Count) << BinaryGroupSize);
        }

        private int GetFreeArea(int Size)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Size);

            if (Size == 0) return Count;

            if (BitMap.TryFindShortestSequence(GetGroupCount(Size), false, GetGroupCount(Count), out var Sequence))
                return Sequence.Start << BinaryGroupSize;

            Capacity += Size;

            return Count;
        }

        private void CopyTo(ArenaCollection<T> Source, int Destination)
        {
            var SourceSpan      = Data.AsSpan(Source.Start, Source.Length);
            var DestinationSpan = Data.AsSpan(Destination, Source.Length);

            SourceSpan.CopyTo(DestinationSpan);
        }

        private bool TryExpand(ArenaCollection<T> Collection, int Additional, out Segment Expanded)
        {
            var SpanEnd = Collection.Start + Collection.Length;
            var Start   = GetGroupCount(SpanEnd);
            var End     = GetGroupCount(SpanEnd + Additional);

            if (Start == End || !BitMap.Contains(Start, End - Start, true))
            {
                Expanded = new Segment(Collection.Start, SpanEnd + Additional - Collection.Start);

                return true;
            }

            Expanded = default;

            return false;
        }
        #endregion
        
        #region Math
        public static int GetGroupCount(int Count)
        {
            return (Count + GroupSize - 1) >> BinaryGroupSize;
        }

        public static int GetFullGroupCount(int Count)
        {
            return Count >> BinaryGroupSize;
        }

        public static int RoundToGroup(int Count)
        {
            return (Count + GroupSize - 1) & ~(GroupSize - 1);
        }

        public static int RoundToBufferSize(int Count)
        {
            return (Count + BufferAccuracy - 1) & ~(BufferAccuracy - 1);
        }
        #endregion
    }
}