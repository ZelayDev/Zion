using System.Collections;

namespace Zion
{
    public abstract class BaseTrie<Node, Leaf> : IEnumerable<Leaf>
    {
        public Node NodeValue;


        internal BaseTrie(Node Node)
        {
            NodeValue = Node;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract IEnumerator<Leaf> GetEnumerator();
    }
}