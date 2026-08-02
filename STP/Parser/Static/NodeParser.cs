namespace Zion.STP
{
    public sealed class NodeParser<Node> where Node : STP.Node
    {
        private readonly INodeReader<Node>[] Readers;
        private readonly INodeErrorHandler<Node> ErrorHandler;
        private readonly int Capacity;


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
            List<Node> Nodes = new List<Node>(Capacity);
            List<Symbol> SemanticTree = new List<Symbol>(10);

            int ErrorCount = 0;

            bool NodeReaded = false;
            int Start = 0;

            while (Start < Tokens.Count)
            {
                NodeReaded = false;

                foreach (INodeReader<Node> Reader in Readers)
                {
                    TokenSlice Slice = new TokenSlice(Tokens, Start);

                    if (Reader.Read(Slice, out Node Node) && Node is not null)
                    {
                        SemanticTree.AddIfNotNull(Node.GetSymbol());

                        Node.ApplyFormat(new TokenSlice(Slice, 0, Node.TokenCount));

                        Start += Node.TokenCount;
                        NodeReaded = true;
                        Nodes.Add(Node);
                        break;
                    }
                }

                if (!NodeReaded)
                {
                    TokenSlice Slice = new TokenSlice(Tokens, Start);

                    Node ErrorNode = ErrorHandler.Handle(Slice);

                    ErrorNode.ApplyFormat(new TokenSlice(Slice, 0, ErrorNode.TokenCount));

                    ErrorCount++;

                    Start += ErrorNode.TokenCount;
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