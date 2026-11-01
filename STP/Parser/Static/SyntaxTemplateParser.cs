namespace Zion.STP
{
    public sealed class SyntaxTemplateParser<Node> where Node : STP.Node
    {
        #region Source
        private readonly TextSource Source;
        #endregion

        #region Tokenization
        private readonly ITokenReader[]     TokenReaders;
        private readonly ITokenErrorHandler TokenErrorHandler;

        private readonly int TokensCapacity;
        #endregion

        #region Nodes
        private readonly NodeParser<Node> NodeParser;
        #endregion

        #region Semantic
        private readonly Func<Symbol>? RootSymbol;
        #endregion

        public SyntaxTemplateParser(TextSource Source, Syntax<Node> Context)
        {
            this.Source = Source.NotNull();

            TokenReaders = Context.TokenReadersArray;
            TokenErrorHandler = Context.TokenErrorHandler;
            TokensCapacity = Context.TokensCapacity;

            NodeParser = new NodeParser<Node>(Context);

            RootSymbol = Context.RootSymbol;
        }


        public ParsingResult<Node> Parse()
        {
            var Tokens = ReadTokens(out var ErrorCollector);
            var Nodes = ReadNodes(Tokens.Tokens);

            HandleSemantic(Tokens, Nodes, ErrorCollector);

            return new ParsingResult<Node>(Tokens, Nodes);
        }


        private TokenParsingResult ReadTokens(out TokenErrorCollector ErrorCollector)
        {
            var Source = this.Source.BeginNew();

            var Tokens = new List<Token>(TokensCapacity);
            var TokensView = new ListView<Token>(Tokens);

            ErrorCollector = new TokenErrorCollector(TokensView);

            while (!Source.IsEnd)
            {
                var TokenReaded = false;

                foreach (var Reader in TokenReaders)
                {
                    var ReaderSource = Source.BeginNew();

                    if (Reader.Read(ref ReaderSource, out var Token) && Token is not null)
                    {
                        ArgumentNullException.ThrowIfNull(ReaderSource);

                        TokenReaded = true;
                        ErrorCollector.AddIfInvalid(Tokens.Count);
                        Tokens.Add(Token);

                        Source = ReaderSource;

                        break;
                    }
                }

                if (!TokenReaded)
                {
                    TokenErrorHandler.Handle(ref Source, out var ErrorToken);

                    if (Tokens.Count != 0 && Tokens[^1] is ErrorToken LastErrorToken)
                    {
                        Tokens[^1] = new ErrorToken
                        {
                            Length = LastErrorToken.Length + ErrorToken.Length
                        };
                    }
                    else
                    {
                        ErrorCollector.AddIfInvalid(Tokens.Count);
                        Tokens.Add(ErrorToken);
                    }
                }
            }

            return new TokenParsingResult(TokensView, ErrorCollector.ErrorsView);
        }

        private NodeParsingResult<Node> ReadNodes(ListView<Token> Tokens)
        {
            return NodeParser.Parse(Tokens);
        }

        private void HandleSemantic(TokenParsingResult Tokens, NodeParsingResult<Node> Nodes,
            TokenErrorCollector                        ErrorCollector)
        {
            var Semantic = Nodes.SemanticData;
            var Start = 0;

            foreach (var Node in Nodes.Nodes)
            {
                var Context = new SemanticContext
                (
                    new TokenSlice(Tokens.Tokens, Start, Node.TokensCount),
                    Start,
                    Semantic,
                    ErrorCollector
                );

                Start += Node.TokensCount;

                Node.Verificate(Context);
            }
        }
    }
}