using System.Collections;

namespace Zion
{
    public abstract class ArenaCollection<T> : IDisposable, IEnumerable<T>
    {
        #region Data
        private readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.SupportsRecursion);

        private Arena<T>? _Source;

        internal Arena<T> Source
        {
            get => _Source ?? throw new ObjectDisposedException(GetType().Name);
            private set => _Source = value;
        }

        internal int Start;
        internal int Length;

        private volatile int Version;
        #endregion

        #region Properties
        public bool IsDisposed => _Source is null;
        #endregion

        #region Constructors
        protected ArenaCollection(ArenaSpan<T> Span)
        {
            Source = Span.Source;
            Start = Span.Start;
            Length = Span.Length;
        }
        #endregion
        
        #region Indexers
        protected T this[int Index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Length);
                Lock.EnterReadLock();
                try
                {
                    return Source[Start + Index];
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Length);
                Lock.EnterReadLock();
                try
                {
                    Modify();
                    Source[Start + Index] = value;
                }
                finally
                {
                    Lock.ExitReadLock();
                }
            }
        }

        protected T this[Index Index]
        {
            get => this[Index.GetOffset(Length)];
            set => this[Index.GetOffset(Length)] = value ?? throw new ArgumentNullException(nameof(value));
        }

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            return IsDisposed
                ? "[Disposed]"
                : StringFormatter.ToString(this);
        }
        #endregion

        #region AbstractMethods
        protected abstract IEnumerator<int> GetIndexEnumerator();

        protected abstract int GetSpanLimit();
        #endregion

        #region PublicMethods
        public bool IsFrom(Arena<T> Arena)
        {
            return ReferenceEquals(_Source, Arena);
        }
        #endregion

        #region ProtectedMethods
        protected void UseSpan(Action<Span<T>> Action)
        {
            Lock.EnterReadLock();
            try
            {
                Modify();
                Action.Invoke(AsSpan());
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected void UseSpan(int Count, Action<Span<T>> Action)
        {
            UseSpan(0, Count, Action);
        }

        protected void UseSpan(int Start, int Count, Action<Span<T>> Action)
        {
            Lock.EnterReadLock();
            try
            {
                Modify();
                Action.Invoke(AsSpan(Start, Count));
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected void UseSpan<I>(I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            Lock.EnterReadLock();
            try
            {
                Modify();
                Action.Invoke(AsSpan(), Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected void UseSpan<I>(int Count, I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            UseSpan(0, Count, Other, Action);
        }

        protected void UseSpan<I>(int Start, int Count, I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            Lock.EnterReadLock();
            try
            {
                Modify();
                Action.Invoke(AsSpan(Start, Count), Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected R UseSpan<R>(Func<Span<T>, R> Function)
        {
            Lock.EnterReadLock();
            try
            {
                Modify();
                return Function.Invoke(AsSpan());
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected R UseSpan<R>(int Count, Func<Span<T>, R> Function)
        {
            return UseSpan(0, Count, Function);
        }

        protected R UseSpan<R>(int Start, int Count, Func<Span<T>, R> Function)
        {
            Lock.EnterReadLock();
            try
            {
                Modify();
                return Function.Invoke(AsSpan(Start, Count));
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected R UseSpan<I, R>(I Other, Func<Span<T>, I, R> Action)
            where I : allows ref struct where R : allows ref struct
        {
            Lock.EnterReadLock();
            try
            {
                Modify();
                return Action.Invoke(AsSpan(), Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected R UseSpan<I, R>(int Count, I Other, Func<Span<T>, I, R> Action)
            where I : allows ref struct where R : allows ref struct
        {
            return UseSpan(0, Count, Other, Action);
        }

        protected R UseSpan<I, R>(int Start, int Count, I Other, Func<Span<T>, I, R> Action)
            where I : allows ref struct where R : allows ref struct
        {
            Lock.EnterReadLock();
            try
            {
                Modify();
                return Action.Invoke(AsSpan(Start, Count), Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected void UseReadOnlySpan(Action<ReadOnlySpan<T>> Action)
        {
            Lock.EnterReadLock();

            try
            {
                ReadOnlySpan<T> ReadOnlySpan = AsSpan();
                Action.Invoke(ReadOnlySpan);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected void UseReadOnlySpan(int Count, Action<ReadOnlySpan<T>> Action)
        {
            UseReadOnlySpan(0, Count, Action);
        }

        protected void UseReadOnlySpan(int Start, int Count, Action<ReadOnlySpan<T>> Action)
        {
            Lock.EnterReadLock();

            try
            {
                ReadOnlySpan<T> ReadOnlySpan = AsSpan(Start, Count);
                Action.Invoke(ReadOnlySpan);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected void UseReadOnlySpan<I>(I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            Lock.EnterReadLock();

            try
            {
                ReadOnlySpan<T> Span = AsSpan();
                Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected void UseReadOnlySpan<I>(int Count, I Other, Action<ReadOnlySpan<T>, I> Action)
            where I : allows ref struct
        {
            UseReadOnlySpan(0, Count, Other, Action);
        }

        protected void UseReadOnlySpan<I>(int Start, int Count, I Other, Action<ReadOnlySpan<T>, I> Action)
            where I : allows ref struct
        {
            Lock.EnterReadLock();

            try
            {
                ReadOnlySpan<T> Span = AsSpan(Start, Count);
                Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected R UseReadOnlySpan<R>(Func<ReadOnlySpan<T>, R> Function)
        {
            Lock.EnterReadLock();

            try
            {
                ReadOnlySpan<T> Span = AsSpan();

                return Function.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected R UseReadOnlySpan<R>(int Count, Func<ReadOnlySpan<T>, R> Function)
        {
            return UseReadOnlySpan(0, Count, Function);
        }

        protected R UseReadOnlySpan<R>(int Start, int Count, Func<ReadOnlySpan<T>, R> Function)
        {
            Lock.EnterReadLock();

            try
            {
                ReadOnlySpan<T> Span = AsSpan(Start, Count);

                return Function.Invoke(Span);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected R UseReadOnlySpan<I, R>(I Other, Func<ReadOnlySpan<T>, I, R> Action)
            where I : allows ref struct where R : allows ref struct
        {
            Lock.EnterReadLock();

            try
            {
                ReadOnlySpan<T> Span = AsSpan();

                return Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        protected R UseReadOnlySpan<I, R>(int Count, I Other, Func<ReadOnlySpan<T>, I, R> Action)
            where I : allows ref struct where R : allows ref struct
        {
            return UseReadOnlySpan(0, Count, Other, Action);
        }

        protected R UseReadOnlySpan<I, R>(int Start, int Count, I Other, Func<ReadOnlySpan<T>, I, R> Action)
            where I : allows ref struct where R : allows ref struct
        {
            Lock.EnterReadLock();

            try
            {
                ReadOnlySpan<T> Span = AsSpan(Start, Count);

                return Action.Invoke(Span, Other);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected void Expand(int Additional)
        {
            EnsureCapacity(Length + Additional);
        }

        protected void EnsureCapacity(int Capacity)
        {
            if (Capacity <= Length)
            {
                return;
            }

            Lock.EnterWriteLock();
            try
            {
                Modify();
                Source.Expand(this, Capacity).Out(out Start, out Length);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        protected void TrimExcess(int Count)
        {
            if (Count >= Length)
            {
                return;
            }

            Lock.EnterWriteLock();
            try
            {
                Modify();
                Source.TrimExcess(this, Count).Out(out Start, out Length);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        protected void Move(int SourceIndex, int DestinationIndex, int Count)
        {
            Lock.EnterWriteLock();
            try
            {
                if (Count <= 0 || SourceIndex == DestinationIndex)
                {
                    return;
                }

                ArgumentOutOfRangeException.ThrowIfWithout(SourceIndex, Length);
                ArgumentOutOfRangeException.ThrowIfWithout(SourceIndex + Count - 1, Length);
                ArgumentOutOfRangeException.ThrowIfWithout(DestinationIndex, Length);
                ArgumentOutOfRangeException.ThrowIfWithout(DestinationIndex + Count - 1, Length);

                Modify();

                var TotalSpan = Source.AsSpan(this);

                if (DestinationIndex > SourceIndex)
                {
                    if (SourceIndex + Count <= DestinationIndex)
                    {
                        TotalSpan.Slice(SourceIndex, Count)
                                 .CopyTo(TotalSpan.Slice(DestinationIndex, Count));
                    }
                    else
                    {
                        for (var i = Count - 1; i >= 0; i--)
                        {
                            TotalSpan[DestinationIndex + i] = TotalSpan[SourceIndex + i];
                        }
                    }
                }
                else
                {
                    if (DestinationIndex + Count <= SourceIndex)
                    {
                        TotalSpan.Slice(SourceIndex, Count)
                                 .CopyTo(TotalSpan.Slice(DestinationIndex, Count));
                    }
                    else
                    {
                        for (var i = 0; i < Count; i++)
                        {
                            TotalSpan[DestinationIndex + i] = TotalSpan[SourceIndex + i];
                        }
                    }
                }
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        protected void CopyTo(Span<T> Destination)
        {
            Lock.EnterWriteLock();
            try
            {
                Source.AsSpan(this).CopyTo(Destination);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        protected void CopyTo(int Start, int Count, Span<T> Destination)
        {
            Lock.EnterReadLock();
            try
            {
                Source.AsSpan(this, Start, Count).CopyTo(Destination);
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }


        protected void Modify()
        {
            Interlocked.Increment(ref Version);
        }


        protected T[] ToArray(int Start, int Count)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Length);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, Length);

            return Source.ToArray(this.Start + Start, Count);
        }
        #endregion

        #region PrivateMethods
        private Span<T> AsSpan()
        {
            return Source.AsSpan(this, 0, GetAbsoluteLimit());
        }

        private Span<T> AsSpan(int Start, int Count)
        {
            var Limit = GetAbsoluteLimit();

            if ((uint)Start > (uint)Limit || (uint)Count > (uint)(Limit - Start))
            {
                throw new IndexOutOfRangeException
                (
                    $"Index out of range: Start(={Start}); Count(={Count}); Limit(={Limit})"
                );
            }

            return Source.AsSpan(this, Start, Count);
        }

        private int GetAbsoluteLimit()
        {
            var Limit = GetSpanLimit();
            var Count = Length;

            return Limit < 0 || Limit > Count
                ? Count
                : Limit;
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Lock.EnterWriteLock();
            try
            {
                if (IsDisposed)
                {
                    return;
                }

                Modify();

                var Arena = Source;
                Source = null!;
                Arena.Release(this);
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }
        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (IsDisposed)
            {
                return EmptyEnumerator<T>.Instance;
            }

            Lock.EnterReadLock();
            try
            {
                return new Enumerator(this, GetIndexEnumerator());
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }
        #endregion

        #region Enumerator
        private sealed class Enumerator : IEnumerator<T>
        {
            private readonly ArenaCollection<T> Collection;
            private readonly IEnumerator<int>   IndexEnumerator;
            private readonly Memory<T>          Memory;
            private readonly int                Version;


            public Enumerator(ArenaCollection<T> Collection, IEnumerator<int> IndexEnumerator)
            {
                this.Collection = Collection;
                this.IndexEnumerator = IndexEnumerator.NotNull();
                Memory = Collection.Source.AsMemory(Collection);
                Version = Collection.Version;
                Current = default!;
            }

            #region IEnumerator<T> Members
            object? IEnumerator.Current => Current;

            public T Current { get; private set; }


            public bool MoveNext()
            {
                CheckVersion();

                if (IndexEnumerator.MoveNext())
                {
                    var Index = IndexEnumerator.Current;
                    Current = Memory.Span[Index];

                    return true;
                }

                return false;
            }

            public void Reset()
            {
                IndexEnumerator.Reset();
                Current = default!;
            }

            public void Dispose()
            {
                IndexEnumerator.Dispose();
            }
            #endregion


            private void CheckVersion()
            {
                if (Collection.Version != Version)
                {
                    throw new InvalidOperationException(
                        "Collection was modified; enumeration operation may not execute.");
                }
            }
        }
        #endregion
    }
}