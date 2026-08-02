using System.Collections;

namespace Zion.STP
{
    public abstract class Node : IEnumerable<Node>
    {
        private List<Node> Childs;

        public Node? Parent { get; private set; }

        public Node Root
        {
            get
            {
                Node Current = this;
                while (Current.Parent is not null)
                {
                    Current = Current.Parent;
                }
                return Current;
            }
        }


        private int Tokens
        {
            get;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

                if (Parent is not null)
                {
                    Parent.Tokens += value - field;
                }

                field = value;
            }
        }

        public int ChildCount => Childs.Count;

        public int TokenCount
        {
            get => Tokens;
            init => Tokens = value;
        }

        public Validation Status
        {
            get;
            protected set
            {
                if (value != field)
                {
                    field = value;
                    StatusChanged?.Invoke(value);
                }
            }
        }


        public event Action<Validation>? StatusChanged;


        public Node()
        {
            Childs = new();
        }


        public Node this[int Index]
        {
            get => Childs[Index];
        }

        public Node this[Index Index]
        {
            get => Childs[Index];
        }


        public virtual Symbol? GetSymbol() => null;

        public virtual void Verificate(SemanticContext Context) { }

        public virtual void ApplyFormat(TokenSlice Tokens) { }


        public bool TryGetParent<T>(out T Result)
        {
            var Current = Parent;

            while (Current is not null)
            {
                if (Current is T Target)
                {
                    Result = Target;
                    return true;
                }
                Current = Current.Parent;
            }

            Result = default!;
            return false;
        }


        public void Add(Node Child)
        {
            ArgumentNullException.ThrowIfNull(Child);
            if (Child.Parent is not null)
            {
                throw new InvalidOperationException("Child already has Parent");
            }

            Child.Add(Child);
        }

        public void Insert(int Index, Node Child)
        {
            Childs.Insert(Index, Child);
            Tokens += Child.TokenCount;
        }

        public void RemoveAt(int Index)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Index, ChildCount);
            Tokens -= Childs[Index].TokenCount;
            Childs.RemoveAt(Index);
        }

        public bool Remove(Node Child)
        {
            if (Childs.Remove(Child))
            {
                Tokens -= Child.TokenCount;
                return true;
            }
            return false;
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Node> GetEnumerator()
        {
            return Childs.GetEnumerator();
        }
    }
}