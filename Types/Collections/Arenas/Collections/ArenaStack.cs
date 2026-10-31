using System.Runtime.InteropServices;

namespace Zion
{
    public sealed class ArenaStack<T> : ArenaCollection<T>
    {
        public bool IsReadOnly => false;

        public int Count
        {
            get;
            private set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                UpdateLimit(value);
                field = value;
            }
        }


        public void Push(T Item)
        {
            SetValueAt(Count++, Item);
        }

        public T Pop()
        {
            return GetValueAt(--Count);
        }

        public T Peek()
        {
            return GetValueAt(Count - 1);
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

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan().Contains(Item, Comparer);
        }

        public bool Remove(T Item, IEqualityComparer<T>? Comparer = null)
        {
            var Span = AsSpan(0, Count);
            var Index = Span.IndexOf(Item, Comparer);

            if (Index < 0)
            {
                return false;
            }

            Copy(Index + 1, Index, Count - Index - 1);
            SetValueAt(Count - 1, default!);
            Count--;
            return true;
        }

        public void Clear()
        {
            ClearAll();
            Count = 0;
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            AsSpan().CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(Span<T> Destination)
        {
            AsSpan().CopyTo(Destination);
        }


        public T[] ToArray()
        {
            var Source = AsSpan();
            var Result = new T[Source.Length];
            
            for (var i = 0; i < Source.Length; i++)
            {
                Result[i] = Source[Source.Length - i - 1];
            }

            return Result;
        }

        public Stack<T> ToStack()
        {
            var Span = AsSpan();
            var Result = new Stack<T>(Span.Length);

            for (var i = Span.Length - 1; i >= 0; i--)
            {
                Result.Push(Span[i]);
            }

            return Result;
        }

        public List<T> ToList()
        {
            var Source = AsSpan();
            var Result = new List<T>(Source.Length);

            CollectionsMarshal.SetCount(Result, Source.Length);
            var Destination = CollectionsMarshal.AsSpan(Result);

            for (var i = 0; i < Source.Length; i++)
            {
                Destination[i] = Source[Source.Length - i - 1];
            }

            return Result;
        }


        public ArenaItemEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(0, Count);
        }
    }
}