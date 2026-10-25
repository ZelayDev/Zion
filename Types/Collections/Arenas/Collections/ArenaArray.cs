namespace Zion
{
    public sealed class ArenaArray<T> : ArenaCollection<T>
    {
        public readonly new int Length;


        public new T this[int Index]
        {
            get => base[Index];
            set => base[Index] = value;
        }

        public new T this[Index Index]
        {
            get => this[Index];
            set => this[Index] = value;
        }


        protected override int GetSpanLimit() => Length;

        protected override IEnumerator<int> GetIndexEnumerator()
        {
            int Count = Length;
            for (int i = 0; i < Count; i++)
            {
                yield return i;
            }
        }


        public T First()
        {
            return this[0];
        }

        public T Last()
        {
            return this[Length - 1];
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Span => Span.IndexOf(Item, Comparer));
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Span => Span.Contains(Item, Comparer));
        }


        public void Clear()
        {
            UseSpan(static Span => Span.Clear());
        }


        public void Reverse()
        {
            UseSpan(static Span => Span.Reverse());
        }

        public void Sort()
        {
            UseSpan(static Span => Span.Sort());
        }


        public new void Expand(int Additional)
        {
            Expand(Additional);
        }


        public T[] ToArray()
        {
            return UseReadOnlySpan(static Span => Span.ToArray());
        }


        public new void UseSpan(Action<Span<T>> Action)
        {
            base.UseSpan(Action);
        }

        public new void UseSpan(int Count, Action<Span<T>> Action)
        {
            base.UseSpan(Count, Action);
        }

        public new void UseSpan(int Start, int Count, Action<Span<T>> Action)
        {
            base.UseSpan(Start, Count, Action);
        }


        public new void UseSpan<I>(I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Other, Action);
        }

        public new void UseSpan<I>(int Count, I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Count, Other, Action);
        }

        public new void UseSpan<I>(int Start, int Count, I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Start, Count, Other, Action);
        }


        public new R UseSpan<R>(Func<Span<T>, R> Function)
        {
            return base.UseSpan(Function);
        }

        public new R UseSpan<R>(int Count, Func<Span<T>, R> Function)
        {
            return base.UseSpan(Count, Function);
        }

        public new R UseSpan<R>(int Start, int Count, Func<Span<T>, R> Function)
        {
            return base.UseSpan(Start, Count, Function);
        }


        public new R UseSpan<I, R>(I Other, Func<Span<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseSpan(Other, Action);
        }

        public new R UseSpan<I, R>(int Count, I Other, Func<Span<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseSpan(Count, Other, Action);
        }

        public new R UseSpan<I, R>(int Start, int Count, I Other, Func<Span<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseSpan(Start, Count, Other, Action);
        }


        public new void UseReadOnlySpan(Action<ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Action);
        }

        public new void UseReadOnlySpan(int Count, Action<ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Count, Action);
        }

        public new void UseReadOnlySpan(int Start, int Count, Action<ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Start, Count, Action);
        }


        public new void UseReadOnlySpan<I>(I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            base.UseReadOnlySpan(Other, Action);
        }

        public new void UseReadOnlySpan<I>(int Count, I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            base.UseReadOnlySpan(Count, Other, Action);
        }

        public new void UseReadOnlySpan<I>(int Start, int Count, I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            base.UseReadOnlySpan(Start, Count, Other, Action);
        }


        public new R UseReadOnlySpan<R>(Func<ReadOnlySpan<T>, R> Function)
        {
            return base.UseReadOnlySpan(Function);
        }

        public new R UseReadOnlySpan<R>(int Count, Func<ReadOnlySpan<T>, R> Function)
        {
            return base.UseReadOnlySpan(Count, Function);
        }

        public new R UseReadOnlySpan<R>(int Start, int Count, Func<ReadOnlySpan<T>, R> Function)
        {
            return base.UseReadOnlySpan(Start, Count, Function);
        }


        public new R UseReadOnlySpan<I, R>(I Other, Func<ReadOnlySpan<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseReadOnlySpan(Other, Action);
        }

        public new R UseReadOnlySpan<I, R>(int Count, I Other, Func<ReadOnlySpan<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseReadOnlySpan(Count, Other, Action);
        }

        public new R UseReadOnlySpan<I, R>(int Start, int Count, I Other, Func<ReadOnlySpan<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseReadOnlySpan(Start, Count, Other, Action);
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(ArenaArray<T> Destination)
        {
            Destination.UseSpan(CopyTo);
        }

        public new void CopyTo(Span<T> Destination)
        {
            base.CopyTo(Destination);
        }
    }
}