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
            return AsSpan().Remove(Item, Comparer);
        }

        public void Clear()
        {
            ClearAll();
            Count = 0;
        }

        
        public void CopyTo(T[] Array, int ArrayIndex)
        {
            throw new NotImplementedException(); //TODO
        }
        
        
        public T[] ToArray()
        {
            T[] Result = new T[Count];
            CopyTo(Result, 0);
            return Result;
        }

        public Stack<T> ToStack()
        {
            return UseSpan
            (
                Count,
                static Span =>
                {
                    var Stack = new Stack<T>(Span.Length);

                    for (int i = Span.Length - 1; i >= 0; i--)
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

            return UseSpan
            (
                Count,
                static Span =>
                {
                    var Result = new List<T>(Span.Length);

                    CollectionsMarshal.SetCount(Result, Span.Length);
                    Span.CopyTo(CollectionsMarshal.AsSpan(Result));

                    return Result;
                }
            );
        }
        
        
        public ArenaItemEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(0, Count);
        }
    }
}