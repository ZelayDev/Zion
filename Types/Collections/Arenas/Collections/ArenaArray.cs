namespace Zion
{
    public sealed class ArenaArray<T> : ArenaCollection<T>
    {
        public int Length => GetLength();


        public T this[int Index]
        {
            get => GetValueAt(Index);
            set => SetValueAt(Index, value);
        }

        public T this[Index Index]
        {
            get => GetValueAt(Index);
            set => SetValueAt(Index, value);
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
            return AsSpan().IndexOf(Item, Comparer);
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan().Contains(Item, Comparer);
        }


        public void Clear()
        {
            ClearAll();
        }
        
        public void Reverse()
        {
            AsSpan().Reverse();
        }

        public void Sort()
        {
            AsSpan().Sort();
        }
        

        public T[] ToArray()
        {
            return AsSpan().ToArray();
        }
        

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(Span<T> Destination)
        {
            AsSpan().CopyTo(Destination);
        }


        public ArenaItemEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(0, Length);
        }
    }
}