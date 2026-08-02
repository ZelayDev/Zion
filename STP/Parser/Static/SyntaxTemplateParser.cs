namespace Zion.STP
{
    public sealed class SyntaxTemplateParser<Node> where Node : STP.Node
    {
        #region Source
        private readonly TextSource Source;

        #endregion

        #region Tokenization
        private readonly ITokenReader[] TokenReaders;
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
            TokenParsingResult Tokens = ReadTokens(out TokenErrorCollector ErrorCollector);
            NodeParsingResult<Node> Nodes = ReadNodes(Tokens.Tokens);

            HandleSemantic(Tokens, Nodes, ErrorCollector);

            return new ParsingResult<Node>(Tokens, Nodes);
        }


        private TokenParsingResult ReadTokens(out TokenErrorCollector ErrorCollector)
        {
            TextSource Source = this.Source.BeginNew();

            List<Token> Tokens = new List<Token>(TokensCapacity);
            ListView<Token> TokensView = new ListView<Token>(Tokens);

            ErrorCollector = new TokenErrorCollector(TokensView);

            while (!Source.IsEnd)
            {
                bool TokenReaded = false;

                foreach (ITokenReader Reader in TokenReaders)
                {
                    TextSource ReaderSource = Source.BeginNew();

                    if (Reader.Read(ref ReaderSource, out Token Token) && Token is not null)
                    {
                        if (ReaderSource is null)
                        {
                            throw new NullReferenceException("After reading the Source token, it cannot be null");
                        }
                        if (Token.Length <= 0)
                        {
                            throw new ParsingException($"Token.Length(={Token.Length}) can not by negative or zero");
                        }

                        TokenReaded = true;
                        ErrorCollector.AddIfInvalid(Tokens.Count);
                        Tokens.Add(Token);

                        Source = ReaderSource;

                        break;
                    }
                }

                if (!TokenReaded)
                {
                    TokenErrorHandler.Handle(ref Source, out ErrorToken ErrorToken);

                    if (Tokens.Count != 0 && Tokens[^1] is ErrorToken LastErrorToken)
                    {
                        Tokens[^1] = new ErrorToken()
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

        private void HandleSemantic(TokenParsingResult Tokens, NodeParsingResult<Node> Nodes, TokenErrorCollector ErrorCollector)
        {
            SemanticData Semantic = Nodes.SemanticData;
            int Start = 0;

            foreach (Node Node in Nodes.Nodes)
            {
                SemanticContext Context = new SemanticContext
                (
                    new TokenSlice(Tokens.Tokens, Start, Node.TokenCount),
                    Start,
                    Semantic,
                    ErrorCollector
                );

                Start += Node.TokenCount;

                Node.Verificate(Context);
            }
        }
    }
}