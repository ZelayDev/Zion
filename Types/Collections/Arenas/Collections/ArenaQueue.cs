namespace Zion
{
    public sealed class ArenaQueue<T> : ArenaCollection<T>, ICollection<T>
    {
        private new int Start;
        private int End;

        public int Count
        {
            get;
            private set
            {
                Modify();
                field = value;
            }
        }

        public bool IsReadOnly => false;


        protected override int GetSpanLimit() => -1;

        protected override IEnumerator<int> GetIndexEnumerator()
        {
            throw new NotImplementedException(); //Do
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
            throw new NotImplementedException(); //Do
        }


        public bool TryPeek(out T Item)
        {
            int Index = Count - 1;
            if (Count == -1)
            {
                Item = default!;
                return false;
            }
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
            Start = 0;
            End = 0;
        }


        public T[] ToArray()
        {
            throw new NotImplementedException(); //Do
        }

        public Queue<T> ToQueue()
        {
            throw new NotImplementedException(); //Do
        }
    }
}