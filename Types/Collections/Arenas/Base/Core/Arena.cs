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
        private readonly IFreeAreaStore              FreeAreaStore;
        private          T[][]                       Chunks;
        #endregion

        #region Properties
        public int Capacity => Chunks.Length << BinaryChunkSize;
        #endregion

        #region Constructors
        public Arena() : this(1) { }

        public Arena(ArenaFreeMode FreeMode)
            : this(1, FreeMode) { }

        public Arena(int Capacity, ArenaFreeMode FreeMode = ArenaFreeMode.RarelyReleased)
            : this(Capacity, GetFreeAreaStore(FreeMode)) { }

        public Arena(int Capacity, IFreeAreaStore Store)
        {
            Collections = new HashSet<ArenaCollection<T>>(ReferenceEqualityComparer.Instance);
            FreeAreaStore = Store.NotNull();
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
            var FreeArea = FreeAreaStore.Allocate(Capacity);

            EnsureCapacity(FreeArea.End);
            
            var Collection = new A();
            var Bridge = new ArenaBridge<T>
            (
                this, new ArenaRegion<T>(Chunks, FreeArea.Start, Capacity)
            );

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

            FreeAreaStore.Clear();
            Collections.Clear();
        }
        #endregion

        #region InternalMethods
        internal ArenaBridge<T> Expand(ArenaCollection<T> Collection, int Capacity)
        {
            CheckCollection(Collection);
            
            var Bridge = Collection.GetBridge();
            var Area = Bridge.GetArea();

            if (Area.Length >= Capacity)
            {
                return Bridge;
            }

            var Expanded = FreeAreaStore.Expand(Area, Capacity);

            EnsureCapacity(Expanded.End);

            return new ArenaBridge<T>(this, new ArenaRegion<T>(Chunks, Expanded));
        }

        internal ArenaBridge<T> Trim(ArenaCollection<T> Collection, int Capacity)
        {
            CheckCollection(Collection);

            var Bridge = Collection.GetBridge();
            var Area = Bridge.GetArea();

            if (Capacity >= Area.Length)
            {
                return Bridge;
            }

            var NewArea = FreeAreaStore.Trim(Area, Capacity);
            
            Clear(Area.Start, Area.Length - NewArea.Length);
            
            return new ArenaBridge<T>
            (
                this, new ArenaRegion<T>(Chunks, NewArea)
            );
        }

        internal void Release(ArenaCollection<T> Collection)
        {
            CheckCollection(Collection);

            var Bridge = Collection.GetBridge();
            var Area = Bridge.GetArea();

            Bridge.Clear();
            FreeAreaStore.Release(Area);
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


        private void Clear(int Start, int Length)
        {
            new ArenaRegion<T>(Chunks, Start, Length).Clear();
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

        private static IFreeAreaStore GetFreeAreaStore(ArenaFreeMode Mode)
        {
            return Mode switch
            {
                // ArenaFreeMode.BumpOnly           =>,
                // ArenaFreeMode.RarelyReleased     =>,
                // ArenaFreeMode.FrequentlyReleased =>,
                _                                => throw new InvalidOperationException("This AreaFreeMode not exists")
            };
        }
        #endregion

        #region Math
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