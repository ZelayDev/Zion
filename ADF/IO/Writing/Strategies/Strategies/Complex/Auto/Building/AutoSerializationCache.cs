namespace Zion.ADF
{
    internal static class AutoSerializationCache
    {
        private static readonly Dictionary<Type, object> Cache = new();


        public static AutoType<T> GetOrAdd<T>(ADFWritingContext Context, Type Type)
        {
            if (!Type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException($"{Type} can not assignable to {typeof(T)}");
            }

            if (Cache.TryGetValue(Type, out var Boxed))
            {
                return (AutoType<T>)Boxed;
            }

            var Created = AutoSerializerBuilder<T>.Create(Context, Type);
            Cache.Add(Type, Created);
            return Created;
        }
    }
}