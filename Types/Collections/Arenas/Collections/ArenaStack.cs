using System.Runtime.InteropServices;

namespace Zion
{
    public sealed class ArenaStack<T> : ArenaCollection<T>, ICollection<T>
    {
        public bool IsReadOnly => false;

        public int Count
        {
            get;
            private set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                Modify();
                Expand(value);
                field = value;
            }
        }


        protected override int GetSpanLimit() => Count;

        protected override IEnumerator<int> GetIndexEnumerator()
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                yield return i;
            }
        }


        public void Push(T Item)
        {
            this[Count++] = Item;
        }

        public T Pop()
        {
            return this[--Count];
        }

        public T Peek()
        {
            return this[Count - 1];
        }


        public bool TryPop(out T Item)
        {
            if (Count > 0)
            {
                Item = Pop();
                return true;
            }
            Item = default!;
            return false;
        }

        public bool TryPeek(out T Item)
        {
            if (Count > 0)
            {
                Item = Peek();
                return true;
            }
            Item = default!;
            return false;
        }


        public void Add(T Item)
        {
            Push(Item);
        }

        public bool Contains(T Item)
        {
            return UseReadOnlySpan
            (
                Span =>
                {
                    var Comparer = EqualityComparer<T>.Default;

                    for (int i = Span.Length - 1; i >= 0; i--)
                    {
                        if (Comparer.Equals(Span[i], Item))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            );
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            UseReadOnlySpan
            (
                Count,
                Span =>
                {
                    var Destination = Array.AsSpan(ArrayIndex);
                    Span.CopyTo(Destination);
                }
            );
        }

        public bool Remove(T Item)
        {
            return UseReadOnlySpan
            (
                Count,
                Span =>
                {
                    var Count = this.Count;
                    var Comparer = EqualityComparer<T>.Default;

                    for (int i = 0; i < Count; i++)
                    {
                        if (Comparer.Equals(Span[i], Item))
                        {
                            Move(i + 1, i, Count - i - 1);
                            this.Count--;
                            return true;
                        }
                    }

                    return false;
                }
            );            
        }

        public void Clear()
        {
            Count = 0;
        }


        public T[] ToArray()
        {
            T[] Result = new T[Count];
            CopyTo(Result, 0);
            return Result;
        }

        public Stack<T> ToStack()
        {
            return UseReadOnlySpan
            (
                Count,
                Span =>
                {
                    var Stack = new Stack<T>(Span.Length);

                    for (int i = Span.Length - 1; i >= 0; i++)
                    {
                        Stack.Push(Span[i]);
                    }

                    return Stack;
                }
            );            
        }

        public List<T> ToList()
        {
            var Count = this.Count;
            if (Count == 0)
            {
                return new List<T>();
            }

            return UseReadOnlySpan
            (
                Count,
                Span =>
                {
                    var Result = new List<T>(Count);

                    CollectionsMarshal.SetCount(Result, Count);
                    Span.CopyTo(CollectionsMarshal.AsSpan(Result));

                    return Result;
                }
            );            
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
    }
}