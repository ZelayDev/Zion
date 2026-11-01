namespace Zion.STP.Dynamic
{
    public sealed class DynamicSyntaxTemplateParser<Node, TPointer>
        where Node : STP.Node where TPointer : TextPointer<TPointer>
    {
        #region Parsing
        #region Source
        private readonly TextDocument<TPointer> Document;
        #endregion

        #region Tokenization
        private readonly ITokenReader[]     TokenReaders;
        private readonly ITokenErrorHandler TokenErrorHandler;

        private readonly int TokensCapacity;
        #endregion

        #region Nodes
        private readonly DynamicNodeParser<Node> NodeReader;
        #endregion

        #region Semantic
        private readonly Func<Symbol>? RootSymbol;
        #endregion
        #endregion

        #region Data
        public ITokenContainer<TPointer> TokenContainer
        {
            private get;
            init => field = value.NotNull();
        } = new RelativeTokenContainer<TPointer>();
        #endregion

        public DynamicSyntaxTemplateParser(TextDocument<TPointer> Document, Syntax<Node> Context)
        {
            this.Document = Document.NotNull();

            TokenReaders = Context.TokenReadersArray;
            TokenErrorHandler = Context.TokenErrorHandler;

            NodeReader = new DynamicNodeParser<Node>(Context);

            TokensCapacity = Context.TokensCapacity;

            RootSymbol = Context.RootSymbol;
        }


        public void Parse()
        {
        }


        public void Change(TextChange<TPointer> Change)
        {
        }
    }
}