namespace Zion
{
    public sealed class ArenaQueue<T> : ArenaCollection<T>
    {
        public int Count => GetLength();

        public bool IsReadOnly => false;


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
            throw new NotImplementedException(); //Do
        }


        public bool TryPeek(out T Item)
        {
            throw new NotImplementedException(); //Do
        }

        public bool TryDequeue(out T Item)
        {
            throw new NotImplementedException(); //Do
        }


        public void Add(T Item)
        {
            Enqueue(Item);
        }

        public bool Contains(T Item)
        {
            throw new NotImplementedException(); //Do
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            throw new NotImplementedException(); //Do
        }

        public bool Remove(T Item)
        {
            throw new NotImplementedException(); //Do
        }

        public void Clear()
        {
            ClearAll();
        }


        public T[] ToArray()
        {
            var Length = Count;
            var Array = new T[Length];

            throw new NotImplementedException(); //Do
            
            return Array;
        }

        public Queue<T> ToQueue()
        {
            throw new NotImplementedException(); //Do
        }
    }
}