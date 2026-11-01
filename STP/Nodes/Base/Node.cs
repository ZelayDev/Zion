namespace Zion.STP
{
    public abstract class Node
    {
        public int TokensCount
        {
            get;
            init => field = value > 0
                ? value
                : throw new ArgumentOutOfRangeException($"Node.TokensCount(={TokensCount}) <= 0");
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

        public virtual Symbol? GetSymbol()
        {
            return null;
        }

        public virtual void Verificate(SemanticContext Context) { }

        public virtual void ApplyFormat(TokenSlice Tokens) { }
    }
}