namespace Zion.ADF
{
    internal sealed class ReferenceIdsRegistry : IWritableRegistry
    {
        private static readonly Reference Null = new Reference(0, new(ADFPrimitives.Object, 0u, -1u));

        private readonly Dictionary<object, Reference> References; //Optimize: Заменить на слабую ссылку
        private readonly List<Reference> NewItems;

        private ulong LastId = 2;

        public bool IsChanged => NewItems.Count > 0;


        public ReferenceIdsRegistry()
        {
            References = new(ReferenceEqualityComparer.Instance);
            NewItems = new();
        }


        public void Write(ADFObjectWriter Writer)
        {
            Writer.Write("Items", NewItems);
            NewItems.Clear();
        }


        public void Add(object Value, DataDefinition Definition)
        {
            var Reference = new Reference(LastId, Definition);

            References.Add(Value, Reference);
            NewItems.Add(Reference);
            LastId += 2;
        }

        public bool TryGetReference(object? Value, out Reference Reference)
        {
            if (Value is null)
            {
                Reference = Null;
                return true;
            }
            return References.TryGetValue(Value, out Reference);
        }
    }
}