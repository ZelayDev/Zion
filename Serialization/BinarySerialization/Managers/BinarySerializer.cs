using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Zion.Serialization
{
    public static class BinarySerializer
    {
        private static readonly ConcurrentDictionary<Type, object> Writers = new();
        private static readonly ConcurrentDictionary<Type, object> Readers = new();


        static BinarySerializer()
        {
            AddSerializer(bool.Serializer);
            AddSerializer(byte.Serializer);
            AddSerializer(sbyte.Serializer);
            AddSerializer(char.Serializer);
            AddSerializer(decimal.Serializer);
            AddSerializer(double.Serializer);
            AddSerializer(float.Serializer);
            AddSerializer(int.Serializer);
            AddSerializer(uint.Serializer);
            AddSerializer(long.Serializer);
            AddSerializer(ulong.Serializer);
            AddSerializer(short.Serializer);
            AddSerializer(ushort.Serializer);
            AddSerializer(string.Serializer);
            AddSerializer(Index.Serializer);
            AddSerializer(Range.Serializer);
            AddSerializer(Half.Serializer);
            AddSerializer(BigInteger.Serializer);
        }


        public static void AddSerializer<T>(IBinarySerializer<T> Serializer)
        {
            AddWriter(Serializer);
            AddReader(Serializer);
        }

        public static void AddWriter<T>(IBinaryWriter<T> ObjectWriter)
        {
            Writers[typeof(T)] = ObjectWriter;
        }

        public static void AddReader<T>(IBinaryReader<T> ObjectReader)
        {
            Readers[typeof(T)] = ObjectReader;
        }


        public static bool ContainsWriter<T>()
        {
            return Writers.ContainsKey(typeof(T));
        }

        public static bool ContainsReader<T>()
        {
            return Readers.ContainsKey(typeof(T));
        }


        public static bool TryGetWriter<T>(out IBinaryWriter<T> Writer)
        {
            if (Writers.TryGetValue(typeof(T), out object? Object))
            {
                Writer = (IBinaryWriter<T>)Object;
                return true;
            }
            Writer = default!;
            return false;
        }

        public static bool TryGetReader<T>(out IBinaryReader<T> Reader)
        {
            if (Readers.TryGetValue(typeof(T), out object? ObjectReader))
            {
                Reader = (IBinaryReader<T>)ObjectReader;
                return true;
            }
            Reader = default!;
            return false;
        }


        public static IBinaryWriter<T>? GetWriter<T>()
        {
            return TryGetWriter(out IBinaryWriter<T> Writer) ? Writer : null;
        }

        public static IBinaryReader<T>? GetReader<T>()
        {
            return TryGetReader(out IBinaryReader<T> Reader) ? Reader : null;
        }


        public static void WriterNotFound<T>([NotNull] IBinaryWriter<T>? ObjectWriter)
        {
            if (ObjectWriter is null)
            {
                WriterNotFound<T>();
            }
        }

        public static void ReaderNotFound<T>([NotNull] IBinaryReader<T>? ObjectReader)
        {
            if (ObjectReader is null)
            {
                ReaderNotFound<T>();
            }
        }

        [DoesNotReturn] public static void WriterNotFound<T>()
        {
            throw new ArgumentException($"Writer for '{typeof(T)}' not found");
        }

        [DoesNotReturn] public static void ReaderNotFound<T>()
        {
            throw new ArgumentException($"Reader for '{typeof(T)}' not found");
        }
    }
}