namespace Zion.STP
{
    public abstract class Node
    {
        public int TokensCount
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

        public virtual Symbol? GetSymbol() => null;

        public virtual void Verificate(SemanticContext Context) { }

        public virtual void ApplyFormat(TokenSlice Tokens) { }
    }
}