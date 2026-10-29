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
            yield return StringRegistry.GetOrAdd
            (
                Type.Assembly?.GetName()?.Name
            );

            foreach (var Id in Split(Type.Namespace))
            {
                yield return Id;
            }

            yield return StringRegistry.GetOrAdd(Type.Name);
        }

        private IEnumerable<uint> Split(string? Path)
        {
            return Path is null
                ? GetNullPath()
                : Path.Split('.').Select(StringRegistry.GetOrAdd);
        }

        private static IEnumerable<uint> GetNullPath()
        {
            yield return StringIdRegistry.Null;
        }
    }
}