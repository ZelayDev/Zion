namespace Zion
{
    public sealed class Trie<Node, Leaf> : BaseTrie<Node, Leaf> where Node : notnull
    {
        #region Data
        private readonly Dictionary<Node, BaseTrie<Node, Leaf>> Childs = new();

        #endregion

        #region Properties
        public int Count => Childs.Count;

        #endregion

        #region Constructors
        public Trie(Node Node)
            : base(Node)
        {
            Childs = new();
        }

        #endregion

        #region Indexers
        public BaseTrie<Node, Leaf> this[Node Node]
        {
            get => Childs[Node];
        }

        #endregion

        #region OverrideMethods
        public override IEnumerator<Leaf> GetEnumerator()
        {
            foreach (var Child in Childs.Values)
            {
                foreach (var Value in Child)
                {
                    yield return Value;
                }
            }
        }

        #endregion

        #region PublicMethods
        public Trie<Node, Leaf> Add(Node Node)
        {
            var Child = new Trie<Node, Leaf>(Node);
            Childs.Add(Node, Child);
            return Child;
        }

        public TrieLeaf<Node, Leaf> Add(Node Node, Leaf Value)
        {
            var Child = new TrieLeaf<Node, Leaf>(Node, Value);
            Childs.Add(Node, Child);
            return Child;
        }

        public TrieLeaf<Node, Leaf> Add(IEnumerable<Node> Path, Leaf Value)
        {
            var Current = this;

            using (var Enumerator = Path.GetEnumerator())
            {
                if (!Enumerator.MoveNext())
                {
                    throw new ArgumentException("Path must contain at least one node.", nameof(Path));
                }

                var CurrentNode = Enumerator.Current;

                while (Enumerator.MoveNext())
                {
                    Current = Current.GetOrAdd(CurrentNode);
                    CurrentNode = Enumerator.Current;
                }

                return Current.Add(CurrentNode, Value);
            }
        }


        public Trie<Node, Leaf> GetOrAdd(Node Node)
        {
            if (Childs.TryGetValue(Node, out var Existing) && Existing is Trie<Node, Leaf> TrieNode)
            {
                return TrieNode;
            }

            var New = new Trie<Node, Leaf>(Node);
            Childs.Add(Node, New);
            return New;
        }


        public bool Remove(Node Node)
        {
            return Childs.Remove(Node);
        }

        public void Clear()
        {
            Childs.Clear();
        }

        #endregion
    }
}