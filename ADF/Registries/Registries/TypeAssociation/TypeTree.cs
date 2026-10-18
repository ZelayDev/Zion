namespace Zion.ADF
{
    internal sealed class TypeTree
    {
        [ADFIgnore] private readonly StringIdRegistry StringRegistry;

        private readonly Trie<uint, uint> Data;

        public bool IsChanged => Data.Count > 0;


        public TypeTree(StringIdRegistry StringRegistry)
        {
            this.StringRegistry = StringRegistry;
            Data = new(StringIdRegistry.Null);
        }


        public void Add(Type Type, uint Id)
        {
            Data.Add(GetTypePath(Type), Id);
        }

        public void Clear()
        {
            Data.Clear();
        }


        private IEnumerable<uint> GetTypePath(Type Type)
        {
            //TODO: Check Null
            foreach (var Id in Split(Type.Assembly.GetName().Name!))
            {
                yield return Id;
            }
            foreach (var Id in Split(Type.FullName!))
            {
                yield return Id;
            }
        }

        private IEnumerable<uint> Split(string Path)
        {
            return Path.Split('.').Select(StringRegistry.GetOrAdd);
        }
    }
}