namespace Zion
{
    public sealed class ArenaQueue<T> : ArenaCollection<T>
    {
        public bool IsReadOnly => false;

        public int Count => GetLength();


        public ArenaQueue()
        {
            throw new NotImplementedException();
        }
        

        public void Enqueue(T Item)
        {
            Add(Item);
        }

        public T Peek()
        {
            if (TryPeek(out T Item))
            {
                return Item;
            }
            throw new IndexOutOfRangeException("Queue is empty");
        }

        public T Dequeue()
        {
            throw new NotImplementedException();
        }


        public bool TryPeek(out T Item)
        {
            throw new NotImplementedException();
        }

        public bool TryDequeue(out T Item)
        {
            throw new NotImplementedException();
        }


        public void Add(T Item)
        {
            Enqueue(Item);
        }

        public bool Contains(T Item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T Item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            ClearAll();
        }


        public T[] ToArray()
        {
            throw new NotImplementedException();
        }

        public Queue<T> ToQueue()
        {
            throw new NotImplementedException();
        }
    }
}