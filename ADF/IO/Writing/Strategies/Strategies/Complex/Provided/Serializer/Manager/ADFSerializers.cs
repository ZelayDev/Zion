namespace Zion.ADF
{
    public static class ADFSerializers
    {
        private static readonly Dictionary<Type, IADFSerializer> Serializers = new();


        public static void Add<T>(Type Type, IADFSerializer<T> Serializer)
        {
            ThrowIfNotAssignable<T>(Type);
            Serializers.Add(Type, Serializer.NotNull());
        }

        public static bool TryGetSerializer<T>(Type Type, out IADFSerializer<T> Serializer)
        {
            ThrowIfNotAssignable<T>(Type);

            if (Serializers.TryGetValue(Type, out var Generalized))
            {
                Serializer = (IADFSerializer<T>)Generalized;
                return true;
            }

            Serializer = default!;
            return false;
        }


        private static void ThrowIfNotAssignable<T>(Type Type)
        {
            if (!Type.IsAssignableFrom(typeof(T)))
            {
                throw new InvalidCastException($"{Type} is not assignable from {typeof(T)}");
            }
        }
    }
}