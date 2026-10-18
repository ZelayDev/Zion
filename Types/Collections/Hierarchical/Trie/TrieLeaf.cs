namespace Zion
{
    public sealed class TrieLeaf<Node, Leaf> : BaseTrie<Node, Leaf>
    {
        public Leaf Value;

        public TrieLeaf(Node Node, Leaf Value) 
            : base(Node)
        {
            this.Value = Value;
        }

        public override IEnumerator<Leaf> GetEnumerator()
        {
            yield return Value;
        }
    }
}