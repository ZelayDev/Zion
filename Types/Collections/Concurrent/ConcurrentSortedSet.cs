using System.Collections;

namespace Zion
{
    public class ConcurrentSortedSet<T> : IEnumerable<T> where T : IComparable<T>
    {
        private readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.NoRecursion);
        private readonly SortedSet<T>         Set  = new();

        public int Count
        {
            get
            {
                Lock.EnterReadLock();
                try
                {
                    return Set.Count;
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }


        public void ForEach(Action<T> Action)
        {
            Lock.EnterReadLock();
            try
            {
                foreach (var item in Set)
                {
                    Action(item);
                }
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public void ForEachInRange(T Min, T Max, Action<T> Action)
        {
            Lock.EnterReadLock();
            try
            {
                foreach (var item in Set.GetViewBetween(Min, Max))
                {
                    Action(item);
                }
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        public bool Add(T Item)
        {
            Lock.EnterWriteLock();
            try
            {
                return Set.Add(Item);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public bool Contains(T Item)
        {
            Lock.EnterReadLock();
            try
            {
                return Set.Contains(Item);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        public bool Remove(T Item)
        {
            Lock.EnterWriteLock();
            try
            {
                return Set.Remove(Item);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public void Clear()
        {
            Lock.EnterWriteLock();
            try
            {
                Set.Clear();
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<T>, IDisposable
        {
            private readonly ConcurrentSortedSet<T>  Parent;
            private          SortedSet<T>.Enumerator NativeEnumerator;
            private          bool                    IsLocked;

            internal Enumerator(ConcurrentSortedSet<T> Parent)
            {
                this.Parent = Parent;
                NativeEnumerator = default;
                IsLocked = false;

                Parent.Lock.EnterReadLock();
                IsLocked = true;

                NativeEnumerator = Parent.Set.GetEnumerator();
            }


            public T Current => NativeEnumerator.Current;

            object IEnumerator.Current => Current;


            public bool MoveNext()
            {
                return NativeEnumerator.MoveNext();
            }

            public void Reset()
            {
                if (IsLocked)
                {
                    NativeEnumerator.Dispose();
                    NativeEnumerator = Parent.Set.GetEnumerator();
                }
            }

            public void Dispose()
            {
                if (IsLocked)
                {
                    Parent.Lock.ExitReadLock();
                    IsLocked = false;
                }
                NativeEnumerator.Dispose();
            }
        }
    }
}