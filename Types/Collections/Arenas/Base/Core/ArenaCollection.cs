namespace Zion
{
    public abstract class ArenaCollection<T> : IDisposable
    {
        #region Data
        private ArenaBridge<T> Bridge;
        private int            Limit;
        #endregion

        #region Properties
        public bool IsDisposed => Bridge.IsDisposed;
        #endregion

        #region PublicMethods
        public bool IsFrom(Arena<T> Arena)
        {
            return Bridge.IsFrom(Arena);
        }
        #endregion

        #region InternalMethods
        internal void Initialize(ArenaBridge<T> Bridge)
        {
            this.Bridge = Bridge;
            UpdateLimit(GetLimit());
            OnInitialized();
        }

        internal ArenaBridge<T> GetBridge()
        {
            return Bridge;
        }

        internal void ResetBridge()
        {
            Bridge = default;
        }
        #endregion

        #region ProtectedMethods
        protected T GetValueAt(int Index)
        {
            ThrowIfDisposed();
            ThrowIfWithout(Index);
            return Bridge[Index];
        }

        protected T GetValueAt(Index Index)
        {
            ThrowIfDisposed();
            return GetValueAt(Index.GetOffset(Limit));
        }

        protected void SetValueAt(int Index, T Value)
        {
            ThrowIfDisposed();
            ThrowIfWithout(Index);
            Bridge[Index] = Value;
        }

        protected void SetValueAt(Index Index, T Value)
        {
            ThrowIfDisposed();
            SetValueAt(Index.GetOffset(Limit), Value);
        }
        

        protected void UpdateLimit(int Limit)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfNegative(Limit);
            this.Limit = Math.Min(Bridge.Length, Limit);
        }


        protected int GetLength()
        {
            return Bridge.Length;
        }

        protected int GetCapacity()
        {
            return Bridge.Length;
        }


        protected void Extend(int NewCapacity)
        {
            ArenaBridge<T>.Expand(this, NewCapacity, ref Bridge);
        }

        protected void Trim(int Length)
        {
            ArenaBridge<T>.Trim(this, Length, ref Bridge);
        }


        protected void ClearAll()
        {
            Bridge.Clear(Limit);
        }

        protected void Move(int SourceIndex, int DestinationIndex, int Count)
        {
            if (Count <= 0 || SourceIndex == DestinationIndex)
            {
                return;
            }

            ArgumentOutOfRangeException.ThrowIfWithout(SourceIndex, Bridge.Length);
            ArgumentOutOfRangeException.ThrowIfWithout(SourceIndex + Count - 1, Bridge.Length);
            ArgumentOutOfRangeException.ThrowIfWithout(DestinationIndex, Bridge.Length);
            ArgumentOutOfRangeException.ThrowIfWithout(DestinationIndex + Count - 1, Bridge.Length);

            var TotalSpan = Bridge.AsSpan();

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


        protected A Allocate<A>(int Capacity) where A : ArenaCollection<T>, new()
        {
            ThrowIfDisposed();
            return Bridge.Allocate<A>(Capacity);
        }

        
        protected ArenaItemEnumerator<T> GetEnumerator(int Start)
        {
            return new ArenaItemEnumerator<T>(Bridge.GetRegion().Slice(Start));
        }

        protected ArenaItemEnumerator<T> GetEnumerator(int Start, int Count)
        {
            return new ArenaItemEnumerator<T>(Bridge.GetRegion().Slice(Start, Count));
        }


        protected void UseSpan(Action<ArenaSpan<T>> Action)
        {
            Action.Invoke(AsSpan());
        }

        protected void UseSpan(int Start, Action<ArenaSpan<T>> Action)
        {
            Action.Invoke(AsSpan(Start));
        }

        protected void UseSpan(int Start, int Count, Action<ArenaSpan<T>> Action)
        {
            Action.Invoke(AsSpan(Start, Count));
        }


        protected void UseSpan<I>(I Other, Action<ArenaSpan<T>, I> Action) where I : allows ref struct
        {
            Action.Invoke(AsSpan(), Other);
        }

        protected void UseSpan<I>(int Start, I Other, Action<ArenaSpan<T>, I> Action) where I : allows ref struct
        {
            Action.Invoke(AsSpan(Start), Other);
        }

        protected void UseSpan<I>(int Start, int Count, I Other, Action<ArenaSpan<T>, I> Action)
            where I : allows ref struct
        {
            Action.Invoke(AsSpan(Start, Count), Other);
        }


        protected R UseSpan<R>(Func<ArenaSpan<T>, R> Function)
        {
            return Function.Invoke(AsSpan());
        }

        protected R UseSpan<R>(int Start, Func<ArenaSpan<T>, R> Function)
        {
            return Function.Invoke(AsSpan(Start));
        }

        protected R UseSpan<R>(int Start, int Count, Func<ArenaSpan<T>, R> Function)
        {
            return Function.Invoke(AsSpan(Start, Count));
        }


        protected R UseSpan<I, R>(I Other, Func<ArenaSpan<T>, I, R> Function)
            where I : allows ref struct where R : allows ref struct
        {
            return Function.Invoke(AsSpan(), Other);
        }

        protected R UseSpan<I, R>(int Start, I Other, Func<ArenaSpan<T>, I, R> Function)
            where I : allows ref struct where R : allows ref struct
        {
            return Function.Invoke(AsSpan(Start), Other);
        }

        protected R UseSpan<I, R>(int Start, int Count, I Other, Func<ArenaSpan<T>, I, R> Function)
            where I : allows ref struct where R : allows ref struct
        {
            return Function.Invoke(AsSpan(Start, Count), Other);
        }


        protected void ThrowIfWithout(int Index)
        {
            if (Index < 0 || Index >= Limit)
            {
                throw new IndexOutOfRangeException(nameof(Index));
            }
        }

        protected void ThrowIfWithout(int Index, int Length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Index);
            ArgumentOutOfRangeException.ThrowIfNegative(Length);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(Index + Length, Limit);
        }

        protected void ThrowIfDisposed()
        {
            if (Bridge.IsDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
        #endregion

        #region VirtualMethods
        protected virtual void OnInitialized() { }

        protected virtual void OnDisposed() { }

        protected virtual int GetLimit()
        {
            return Bridge.Length;
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (!IsDisposed)
            {
                OnDisposed();
                Bridge.Dispose(this);
                Bridge = default!;
            }
        }
        #endregion

        #region PrivateMethods
        private protected ArenaSpan<T> AsSpan()
        {
            ThrowIfDisposed();
            return Bridge.AsSpan().Slice(0, Limit);
        }

        private protected ArenaSpan<T> AsSpan(int Start)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfGreaterThan(Start, Limit, nameof(Start));
            return Bridge.AsSpan().Slice(Start, Limit - Start);
        }

        private protected ArenaSpan<T> AsSpan(int Start, int Count)
        {
            ThrowIfDisposed();
            ArgumentOutOfRangeException.ThrowIfGreaterThan(Start + Count, Limit, nameof(Count));
            return Bridge.AsSpan().Slice(Start, Count);
        }
        #endregion
    }
}