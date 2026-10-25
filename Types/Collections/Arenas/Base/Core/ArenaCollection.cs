namespace Zion
{
    public abstract class ArenaCollection<T> : IDisposable
    {
        #region Data
        private ArenaBridge<T> Bridge;
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
            return Bridge[Index];
        }

        protected T GetValueAt(Index Index)
        {
            return Bridge[Index];
        }

        protected void SetValueAt(int Index, T Value)
        {
            Bridge[Index] = Value;
        }

        protected void SetValueAt(Index Index, T Value)
        {
            Bridge[Index] = Value;
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

        protected void CopyTo(Span<T> Destination)
        {
            Bridge.AsSpan().CopyTo(Destination);
        }

        protected void CopyTo(int Start, int Count, Span<T> Destination)
        {
            Bridge.AsSpan().Slice(Start, Count).CopyTo(Destination);
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
        #endregion

        #region VirtualMethods
        protected virtual void OnDisposed() { }
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
        private ArenaSpan<T> AsSpan()
        {
            return Bridge.AsSpan();
        }

        private ArenaSpan<T> AsSpan(int Start)
        {
            return Bridge.AsSpan().Slice(Start);
        }

        private ArenaSpan<T> AsSpan(int Start, int Count)
        {
            return Bridge.AsSpan().Slice(Start, Count);
        }
        #endregion
    }
}