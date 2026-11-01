namespace Zion
{
    public sealed class ConcurrentObjectPool<T>
    {
        #region Data
        private readonly List<T> Pool;
        private readonly Lock    Lock = new();

        public required Func<T> New
        {
            get;
            init => field = value.NotNull();
        }

        public Action<T>? Remove { get; init; }

        public Func<T, T> Getter
        {
            get;
            init => field = value ?? BaseGet;
        } = BaseGet;
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                lock (Lock)
                {
                    return Pool.Count;
                }
            }
        }
        #endregion

        #region Constructors
        public ConcurrentObjectPool()
        {
            Pool = new List<T>(8);
        }

        public ConcurrentObjectPool(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
            Pool = new List<T>(Capacity);
        }
        #endregion

        #region PublicMethods
        public void Add(T Item)
        {
            lock (Lock)
            {
                Pool.Add(Item);
            }
        }

        public void Add(IEnumerable<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);

            lock (Lock)
            {
                Pool.AddRange(Items);
            }
        }


        public T Get()
        {
            lock (Lock)
            {
                if (Pool.Count != 0)
                {
                    var Item = Getter(Pool[^1]);
                    Pool.RemoveAt(Pool.Count - 1);
                    return Item;
                }
            }

            return New();
        }

        public bool TryGet(out T Item)
        {
            lock (Lock)
            {
                if (Pool.Count > 0)
                {
                    Item = Getter(Pool[^1]);
                    Pool.RemoveAt(Pool.Count - 1);
                    return true;
                }
            }

            Item = default!;
            return false;
        }


        public void Clear()
        {
            lock (Lock)
            {
                if (Remove is not null)
                {
                    foreach (var Item in Pool)
                    {
                        Remove(Item);
                    }
                }
                Pool.Clear();
            }
        }


        public void ProcessBatch(int MaxConcurrency, IEnumerable<Action<T>> Actions)
        {
            ArgumentNullException.ThrowIfNull(Actions);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxConcurrency);

            using SemaphoreSlim Semaphore = new(MaxConcurrency, MaxConcurrency);
            List<Task> Tasks = new();

            foreach (var Action in Actions)
            {
                Semaphore.Wait();

                Tasks.Add(Task.Run(() =>
                {
                    T Resource = default!;
                    try
                    {
                        Resource = Get();
                        Action(Resource);
                    }
                    finally
                    {
                        if (Resource is not null)
                        {
                            Add(Resource);
                        }
                        Semaphore.Release();
                    }
                }));
            }

            Task.WaitAll(Tasks);
        }
        #endregion

        #region PrivateMethods
        private static T BaseGet(T Value)
        {
            return Value;
        }
        #endregion
    }
}