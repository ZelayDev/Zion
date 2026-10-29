namespace Zion.ADF
{
    internal sealed class TypeAssociation : IWritableRegistry
    {
        private readonly Dictionary<Type, uint> Data;

        private readonly TypeTree NewItems;

        public bool IsChanged => NewItems.IsChanged;


        public TypeAssociation(StringIdRegistry StringRegistry)
        {
            Data = new();
            NewItems = new(StringRegistry);
        }


        public uint this[Type Type] => Data[Type];


        public void Write(ADFObjectWriter Writer)
        {
            Writer.Write("NewItems", NewItems);
            NewItems.Clear();
        }


        public bool TryGetFormatId(Type Type, out uint FormatId)
        {
            return Data.TryGetValue(Type, out FormatId);
        }

        public bool Add(Type Type, uint FormatId)
        {
            NewItems.Add(Type, FormatId);
            return Data.TryAdd(Type, FormatId);
        }

        public uint GetOrAdd(Type Type, Func<uint> Fabric)
        {
            if (Data.TryGetValue(Type, out var Cached))
            {
                return Cached;
            }

            var Id = Fabric();

            NewItems.Add(Type, Id);
            Data.Add(Type, Id);

            return Id;
        }

        public uint GetOrAddDeferred(Type? Type, FormatRegistry FormatRegistry)
        {
            if (Type is null)
            {
                return ADFPrimitives.Object;
            }

            if (Data.TryGetValue(Type, out var Existing))
            {
                return Existing;
            }

            var Base = DataFormat.HasBase(Type) 
                ? GetOrAddDeferred(Type.BaseType, FormatRegistry)
                : ADFPrimitives.Object;

            var Id = FormatRegistry.AddDeferred(Base);

            NewItems.Add(Type, Id);
            Data.Add(Type, Id);

            return Id;
        }
    }
}