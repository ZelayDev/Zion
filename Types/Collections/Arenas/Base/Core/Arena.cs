using System.Runtime.CompilerServices;

namespace Zion
{
    public sealed class Arena<T>
    {
        #region Constants
        internal const int BinaryChunkSize = 12;
        internal const int ChunkSize       = 1 << BinaryChunkSize;
        #endregion

        #region Data
        private readonly HashSet<ArenaCollection<T>> Collections;
        private readonly SortedList<Segment>         FreeAreas;
        private          T[][]                       Chunks;

        private int UsedCount;
        #endregion

        #region Properties
        public int Capacity => Chunks.Length << BinaryChunkSize;
        #endregion

        #region Constructors
        public Arena() : this(1) { }

        public Arena(int Capacity)
        {
            Collections = new HashSet<ArenaCollection<T>>(ReferenceEqualityComparer.Instance);
            FreeAreas = new SortedList<Segment>(Segment.ByLengthComparer);
            Chunks = GetChunks(Capacity);
        }
        #endregion

        #region PublicMethods
        public ArenaArray<T> GetArray(int Capacity)
        {
            return Allocate<ArenaArray<T>>(Capacity);
        }

        public ArenaBuffer<T> GetBuffer(int Capacity)
        {
            return Allocate<ArenaBuffer<T>>(Capacity);
        }

        public ArenaList<T> GetList(int Capacity)
        {
            return Allocate<ArenaList<T>>(Capacity);
        }

        public ArenaStack<T> GetStack(int Capacity)
        {
            return Allocate<ArenaStack<T>>(Capacity);
        }

        public ArenaQueue<T> GetQueue(int Capacity)
        {
            return Allocate<ArenaQueue<T>>(Capacity);
        }


        public A Allocate<A>(int Capacity) where A : ArenaCollection<T>, new()
        {
            var Bridge = GetNewBridge(Capacity);
            var Collection = new A();

            Collection.Initialize(Bridge);
            Collections.Add(Collection);

            return Collection;
        }


        public void EnsureCapacity(int NewCapacity)
        {
            if (Capacity < NewCapacity)
            {
                Expand(ref Chunks, GetChunkCount(NewCapacity));
            }
        }

        public void Clear()
        {
            foreach (var Collection in Collections)
            {
                Collection.ResetBridge();
            }

            Collections.Clear();
            FreeAreas.Clear();
            UsedCount = 0;
        }
        #endregion

        #region InternalMethods
        internal ArenaBridge<T> Expand(ArenaCollection<T> Collection, int Capacity)
        {
            CheckCollection(Collection);

            var Bridge = Collection.GetBridge();
            var Area = Bridge.GetArea();

            if (Area.End == UsedCount)
            {
                var Addition = Capacity - Bridge.Length;
                UsedCount += Addition;
                EnsureCapacity(UsedCount);
                return new ArenaBridge<T>
                (
                    this, new ArenaRegion<T>(Chunks, Area.Start, Capacity)
                );
            }

            AddFreeArea(Area);
            return GetNewBridge(Capacity);
        }

        internal void Release(ArenaCollection<T> Collection)
        {
            CheckCollection(Collection);

            var Bridge = Collection.GetBridge();
            var Area = Bridge.GetArea();

            Bridge.Clear();
            
            if (Area.End == UsedCount)
            {
                UsedCount -= Area.Length;
            }
            else
            {
                AddFreeArea(Area);
            }
        }
        #endregion

        #region PrivateMethods
        private void CheckCollection(ArenaCollection<T> Collection)
        {
            if (Collection is null)
            {
                throw new ArgumentNullException(nameof(Collection));
            }
            if (!Collection.IsFrom(this))
            {
                throw new ArgumentException("Collection is not from the arena collection.");
            }
            if (Collection.IsDisposed)
            {
                throw new ObjectDisposedException(Collection.GetType().FullName);
            }
        }

        private ArenaBridge<T> GetNewBridge(int Capacity)
        {
            var FreeArea = GetFreeArea(Capacity);
            return new ArenaBridge<T>
            (
                this, new ArenaRegion<T>(Chunks, FreeArea.Start, Capacity)
            );
        }

        private Segment GetFreeArea(int Capacity)
        {
            if (Capacity == 0)
            {
                return Segment.Empty;
            }

            if (FreeAreas.TryFindFirstAtLeast(new Segment(0, Capacity), out var AreaIndex))
            {
                var Start = FreeAreas[AreaIndex].Start;
                return new Segment(Start, Capacity);
            }

            var Result = new Segment(UsedCount, Capacity);
            UsedCount += Capacity;
            return Result;
        }

        private void AddFreeArea(Segment Area)
        {
            throw new NotImplementedException(); //TODO: Добавить с учётом совмещения 2 рядом стоящих областей
        }


        private static T[][] GetChunks(int Capacity)
        {
            var ChunkCount = GetChunkCount(Capacity);
            var Chunks = new T[][ChunkCount];

            for (var i = 0; i < ChunkCount; i++)
            {
                Chunks[i] = new T[ChunkSize];
            }

            return Chunks;
        }

        private static void Expand(ref T[][] Chunks, int ChunkCount)
        {
            if (ChunkCount < Chunks.Length)
            {
                return;
            }

            var OldLength = Chunks.Length;

            Array.Resize(ref Chunks, ChunkCount);

            for (var i = OldLength; i < ChunkCount; i++)
            {
                Chunks[i] = new T[ChunkSize];
            }
        }
        #endregion

        #region Math
        public static T[][] Slice(T[][] Chunks, int Start, int Length)
        {
            if (Length == 0)
            {
                return [];
            }

            var First = GetChunkIndex(Start);
            var Last = GetChunkIndex(Start + Length - 1);
            
            return First == 0 && Last == Chunks.Length - 1
                ? Chunks
                : ZArray.GetSubArray(Chunks, First, Last - First + 1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetChunkCount(int Count)
        {
            return (Count + ChunkSize - 1) >> BinaryChunkSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetChunkIndex(int Index)
        {
            return Index >> BinaryChunkSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetLocalIndex(int Index)
        {
            return Index & (ChunkSize - 1);
        }
        #endregion
    }
}