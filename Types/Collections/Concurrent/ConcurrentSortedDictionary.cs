namespace Zion
{
    public class ConcurrentSortedDictionary<TKey, TValue> : IDisposable where TKey : IComparable<TKey>
    {
        #region Data
        private readonly ReaderWriterLockSlim           Lock       = new();
        private readonly SortedDictionary<TKey, TValue> Dictionary = new();
        #endregion

        #region Properties
        public bool IsEmpty
        {
            get
            {
                Lock.EnterReadLock();
                try
                {
                    return Dictionary.Count == 0;
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }

        public int Count
        {
            get
            {
                Lock.EnterReadLock();
                try
                {
                    return Dictionary.Count;
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }


        public ICollection<TKey> Keys
        {
            get
            {
                Lock.EnterReadLock();
                try
                {
                    return Dictionary.Keys.ToArray();
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                Lock.EnterReadLock();
                try
                {
                    return Dictionary.Values.ToArray();
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }
        #endregion

        #region Public Methods
        public bool TryGetValue(TKey Key, out TValue Value)
        {
            Lock.EnterReadLock();
            try
            {
                return Dictionary.TryGetValue(Key, out Value);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public bool TryAdd(TKey Key, TValue Value)
        {
            Lock.EnterWriteLock();
            try
            {
                return Dictionary.TryAdd(Key, Value);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public bool TryRemove(TKey Key, out TValue Value)
        {
            Lock.EnterWriteLock();
            try
            {
                return Dictionary.Remove(Key, out Value!);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }


        public TValue GetOrAdd(TKey Key, Func<TKey, TValue> ValueFactory)
        {
            Lock.EnterReadLock();
            try
            {
                if (Dictionary.TryGetValue(Key, out var ExistingValue))
                {
                    return ExistingValue;
                }
            }
            finally
            {
                Lock.ExitReadLock();
            }

            Lock.EnterWriteLock();
            try
            {
                if (Dictionary.TryGetValue(Key, out var ExistingValue))
                {
                    return ExistingValue;
                }

                var NewValue = ValueFactory(Key);
                Dictionary[Key] = NewValue;
                return NewValue;
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public void AddOrUpdate(TKey Key, TValue AddValue, Func<TKey, TValue, TValue> UpdateFactory)
        {
            Lock.EnterWriteLock();
            try
            {
                if (Dictionary.TryGetValue(Key, out var existingValue))
                {
                    Dictionary[Key] = UpdateFactory(Key, existingValue);
                }
                else
                {
                    Dictionary[Key] = AddValue;
                }
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }


        public bool ContainsKey(TKey Key)
        {
            Lock.EnterReadLock();
            try
            {
                return Dictionary.ContainsKey(Key);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        public TKey? GetLastKey()
        {
            Lock.EnterReadLock();
            try
            {
                return Dictionary.Count > 0 ? Dictionary.Keys.Last() : default;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public TKey? GetFirstKey()
        {
            Lock.EnterReadLock();
            try
            {
                return Dictionary.Count > 0 ? Dictionary.Keys.First() : default;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        public void Clear()
        {
            Lock.EnterWriteLock();
            try
            {
                Dictionary.Clear();
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerable()
        {
            KeyValuePair<TKey, TValue>[] Snapshot;
            Lock.EnterReadLock();
            try
            {
                Snapshot = Dictionary.ToArray();
            }
            finally
            {
                Lock.ExitReadLock();
            }

            foreach (var kvp in Snapshot)
            {
                yield return kvp;
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Lock.Dispose();
        }
        #endregion
    }
}