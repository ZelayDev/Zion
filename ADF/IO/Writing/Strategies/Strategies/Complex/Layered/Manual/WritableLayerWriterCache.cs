using System.Linq.Expressions;
using System.Reflection;

namespace Zion.ADF
{
    internal static class WritableLayerWriterCache<T>
    {
        private static readonly Dictionary<Type, bool> InterfaceCache = new();
        private static readonly Dictionary<Type, WritableLayerWriter<T>> Cache = new();


        public static bool TryGetSerializer(Type LayerType, out IADFSerializer<T> Serializer)
        {
            if (IsWritable(LayerType))
            {
                Serializer = GetOrAdd(LayerType);
                return true;
            }

            Serializer = default!;
            return false;
        }


        private static WritableLayerWriter<T> GetOrAdd(Type LayerType)
        {
            return Cache.GetOrAdd(LayerType, BuildWriter);
        }

        private static WritableLayerWriter<T> BuildWriter(Type LayerType)
        {
            var Map    = LayerType.GetInterfaceMap(typeof(IADFWritable));
            var Method = Map.TargetMethods.First(static Method => Method.Name == nameof(IADFWritable.Write));
            var Action = BuildWriteDelegate(LayerType, Method);

            return new(Action);
        }

        private static Action<ADFObjectWriter, T> BuildWriteDelegate(Type LayerType, MethodInfo Method)
        {
            var WriterParameter = Expression.Parameter(typeof(ADFObjectWriter), "Writer");
            var ValueParameter  = Expression.Parameter(typeof(T), "Value");
            var CastValue       = Expression.Convert(ValueParameter, LayerType);
            var Call            = Expression.Call(CastValue, Method, WriterParameter);

            return Expression.Lambda<Action<ADFObjectWriter, T>>(Call, WriterParameter, ValueParameter).Compile();
        }


        private static bool IsWritable(Type LayerType)
        {
            if (!LayerType.IsAssignableTo(typeof(IADFWritable)))
            {
                return false;
            }

            if (InterfaceCache.TryGetValue(LayerType, out var Cached))
            {
                return Cached;
            }

            var Map = LayerType.GetInterfaceMap(typeof(IADFWritable));
            var Result = false;

            foreach (var Method in Map.TargetMethods)
            {
                if (Method.DeclaringType == LayerType)
                {
                    Result = true;
                    break;
                }
            }

            InterfaceCache[LayerType] = Result;
            return Result;
        }
    }
}