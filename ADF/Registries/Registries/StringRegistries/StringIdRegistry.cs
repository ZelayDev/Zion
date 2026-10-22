namespace Zion.ADF
{
    internal sealed class StringIdRegistry : IWritableRegistry
    {
        public const uint Null = 0u;

        private readonly Dictionary<string, uint> Data;
        private readonly List<string> NewItems;

        private uint LastId = 1;

        public bool IsChanged => NewItems.Count > 0;


        public StringIdRegistry()
        {
            Data = new();
            NewItems = new();
        }


        public void Write(ADFObjectWriter Writer)
        {
            Writer.WriteStrings("NewItems", NewItems);
            NewItems.Clear();
        }


        public uint GetOrAdd(string? String)
        {
            if (String is null)
            {
                return Null;
            }
            if (Data.TryGetValue(String, out uint Id))
            {
                return Id;
            }
            Data.Add(String, LastId);
            NewItems.Add(String);
            return LastId++;
        }

        public bool TryGetId(string? String, out uint Id)
        {
            if (String is null)
            {
                Id = Null;
                return true;
            }
            return Data.TryGetValue(String, out Id);
        }


        public string? GetString(in uint Id)
        {
            if (Id == Null)
            {
                return null;
            }

            if (Id >= LastId)
            {
                throw new KeyNotFoundException($"String with id '{Id}' not exists");
            }

            foreach (var Pair in Data)
            {
                if (Pair.Value == Id)
                {
                    return Pair.Key;
                }
            }

            throw new KeyNotFoundException($"String with id '{Id}' not exists");
        }
    }
}