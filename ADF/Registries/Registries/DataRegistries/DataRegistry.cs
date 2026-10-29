namespace Zion.ADF
{
    internal sealed class DataRegistry : IRegistry
    {
        private readonly StringIdRegistry StringRegistry;
        private readonly Dictionary<uint, DataDefinition> Parameters;
        private readonly List<(uint, DataDefinition)> NewItems;

        public bool IsChanged => NewItems.Count > 0;


        public DataRegistry(StringIdRegistry StringRegistry)
        {
            this.StringRegistry = StringRegistry.NotNull();
            Parameters = new();
            NewItems   = new();
        }


        public void Write(ADFObjectWriter Writer)
        {
            Writer.Write("Items", NewItems);
            NewItems.Clear();
        }


        public void Add(string Name, DataDefinition Definition)
        {
            if (!TryAdd(Name, Definition))
            {
                throw new ADFRepeatedNameException(Name);
            }
        }

        public void Add(string Name, uint NameId, DataDefinition Definition)
        {
            if (!TryAdd(NameId, Definition))
            {
                throw new ADFRepeatedNameException(Name);
            }
        }

        public bool TryAdd(string Name, DataDefinition Definition)
        {
            return TryAdd(StringRegistry.GetOrAdd(Name), Definition);
        }

        public bool TryAdd(uint NameId, DataDefinition Definition)
        {
            if (!Parameters.TryAdd(NameId, Definition))
            {
                return false;
            }
            NewItems.Add((NameId, Definition));
            return true;
        }


        public bool Contains(string Name)
        {
            return StringRegistry.TryGetId(Name, out uint Id)
                && Parameters.ContainsKey(Id);
        }

        public bool Contains(uint NameId)
        {
            return Parameters.ContainsKey(NameId);
        }


        public bool TryGetDefinition(string Name, out DataDefinition Definition)
        {
            if (StringRegistry.TryGetId(Name, out uint Id)
                && Parameters.TryGetValue(Id, out Definition))
            {
                return true;
            }

            Definition = default;
            return false;
        }
    }
}