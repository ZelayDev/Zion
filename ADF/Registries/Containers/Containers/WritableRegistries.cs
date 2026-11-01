using System.Collections;

namespace Zion.ADF
{
    internal sealed class WritableRegistries : IEnumerable<WritableRegistryInfo>
    {
        private readonly Dictionary<string, WritableRegistryInfo> Registries;

        public readonly ReferenceIdsRegistry References;
        public readonly StringIdRegistry     StringRegistry;
        public readonly TypeAssociation      TypeAssociation;
        public readonly FormatRegistry       FormatRegistry;
        public readonly DataRegistry         DataRegistry;

        public ushort LastRegistryId = 32;


        public WritableRegistries()
        {
            References = new ReferenceIdsRegistry();
            StringRegistry = new StringIdRegistry();
            FormatRegistry = new FormatRegistry();
            TypeAssociation = new TypeAssociation(StringRegistry);
            DataRegistry = new DataRegistry(StringRegistry);
            Registries = new Dictionary<string, WritableRegistryInfo>
            {
                { "StringRegistry", new WritableRegistryInfo(1, StringRegistry) },
                { "FormatRegistry", new WritableRegistryInfo(2, FormatRegistry) },
                { "DataRegistry", new WritableRegistryInfo(3, DataRegistry) },
                { "References", new WritableRegistryInfo(4, References) },
                { "TypeAssociation", new WritableRegistryInfo(5, TypeAssociation) }
            };
        }


        public bool TryGetRegistry<T>(string Name, out T Registry) where T : IWritableRegistry
        {
            if (Registries.TryGetValue(Name, out var WritableRegistry)
                && WritableRegistry is T Target)
            {
                Registry = Target;
            }
            Registry = default!;
            return false;
        }

        public void Add(string Name, IWritableRegistry Registry)
        {
            Registries.Add
            (
                Name.NotNull(),
                new WritableRegistryInfo(LastRegistryId++, Registry)
            );
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<WritableRegistryInfo> GetEnumerator()
        {
            return Registries.Values.GetEnumerator();
        }
    }
}