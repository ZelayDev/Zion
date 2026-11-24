namespace Zion.STP
{
    public readonly struct SkipToBreakToken<Node> : INodeErrorHandler<Node> where Node : STP.Node, new()
    {
        private readonly HashSet<Type> BreakTokens;

        public SkipToBreakToken(IEnumerable<Type> BreakTokens)
        {
            this.BreakTokens = BreakTokens.ToHashSet();
        }

        public Node Handle(TokenSlice Tokens)
        {
            for (int i = 0; i < Tokens.Count; i++)
            {
                if (BreakTokens.Contains(Tokens[i].GetType()))
                {
                    return new Node() { TokenCount = i };
                }
            }

            return new Node() { TokenCount = Tokens.Count - 1 };
        }
    }
}