namespace Zion.STP.Dynamic
{
    public sealed class DynamicNodeParser<Node> where Node : STP.Node
    {
        private readonly INodeReader<Node>[]     Readers;
        private readonly INodeErrorHandler<Node> ErrorHandler;
        private readonly int                     Capacity;

        public DynamicNodeParser(Syntax<Node> Context)
        {
            Readers = Context.NodeReadersArray;
            ErrorHandler = Context.NodeErrorHandler;
            Capacity = Context.NodesCapacity;
        }

        public DynamicNodeParser(NodeSyntax<Node> Context)
        {
            Readers = Context.ReadersArray;
            ErrorHandler = Context.ErrorHandler;
            Capacity = Context.Capacity;
        }
    }
}