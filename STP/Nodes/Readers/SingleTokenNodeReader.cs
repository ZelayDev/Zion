namespace Zion.STP
{
    public sealed class SingleTokenNodeReader<Node> : INodeReader<Node> where Node : STP.Node
    {
        private readonly ITokenMatcher     TokenMatcher;
        private readonly Func<Token, Node> NodeGetter;

        public SingleTokenNodeReader(ITokenMatcher TokenMatcher, Func<Token, Node> NodeGetter)
        {
            this.TokenMatcher = TokenMatcher.NotNull();
            this.NodeGetter = NodeGetter.NotNull();
        }

        public bool Read(TokenSlice Tokens, out Node Node)
        {
            var Token = Tokens[0];

            var Context = new MatchingContext();

            if (TokenMatcher.Match(Token, Context))
            {
                ArgumentException.ThrowIf(!Context.GoToNext,
                    "SingleTokenNodeReader requires GoToNext = true. The token matched but refused to advance.");

                Node = NodeGetter(Token);
                return true;
            }

            Node = default!;
            return false;
        }
    }
}