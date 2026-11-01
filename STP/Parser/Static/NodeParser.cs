namespace Zion.STP
{
    public sealed class NodeParser<Node> where Node : STP.Node
    {
        private readonly INodeReader<Node>[]     Readers;
        private readonly INodeErrorHandler<Node> ErrorHandler;
        private readonly int                     Capacity;


        public NodeParser(Syntax<Node> Context)
        {
            Readers = Context.NodeReadersArray;
            ErrorHandler = Context.NodeErrorHandler;
            Capacity = Context.NodesCapacity;
        }

        public NodeParser(NodeSyntax<Node> Context)
        {
            Readers = Context.ReadersArray;
            ErrorHandler = Context.ErrorHandler;
            Capacity = Context.Capacity;
        }


        public NodeParsingResult<Node> Parse(ListView<Token> Tokens)
        {
            var Nodes = new List<Node>(Capacity);
            var SemanticTree = new List<Symbol>(10);

            var ErrorCount = 0;

            var NodeReaded = false;
            var Start = 0;

            while (Start < Tokens.Count)
            {
                NodeReaded = false;

                foreach (var Reader in Readers)
                {
                    var Slice = new TokenSlice(Tokens, Start);

                    if (Reader.Read(Slice, out var Node) && Node is not null)
                    {
                        SemanticTree.AddIfNotNull(Node.GetSymbol());

                        Node.ApplyFormat(new TokenSlice(Slice, 0, Node.TokensCount));

                        Start += Node.TokensCount;
                        NodeReaded = true;
                        Nodes.Add(Node);
                        break;
                    }
                }

                if (!NodeReaded)
                {
                    var Slice = new TokenSlice(Tokens, Start);

                    var ErrorNode = ErrorHandler.Handle(Slice);

                    ErrorNode.ApplyFormat(new TokenSlice(Slice, 0, ErrorNode.TokensCount));

                    ErrorCount++;

                    Start += ErrorNode.TokensCount;
                    Nodes.Add(ErrorNode);
                }
            }

            return new NodeParsingResult<Node>
            (
                Nodes,
                new SemanticData(SemanticTree),
                ErrorCount
            );
        }
    }
}