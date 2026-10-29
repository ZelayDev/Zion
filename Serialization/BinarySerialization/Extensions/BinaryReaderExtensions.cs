namespace Zion.Serialization
{
    public static class BinaryReaderExtensions
    {
        extension(BinaryReader Reader)
        {
            public T Read<T>() where T : IBinaryReadable<T>
            {
                return T.Read(Reader);
            }

            public T Read<T>(IBinaryReader<T>? ObjectReader = null)
            {
                ObjectReader ??= BinarySerializer.GetReader<T>();

                if (ObjectReader is null)
                {
                    throw new ArgumentException($"The basic Reader for {typeof(T).FullName} was not found");
                }

                return ObjectReader.Read(Reader);
            }


            public T Read<T, I>() where T : IBinaryReadable<T, I> where I : IBinaryReadable<I>
            {
                return T.Read(Reader, () => I.Read(Reader));
            }

            public T Read<T, I>(Func<BinaryReader, I> ObjectReader) where T : IBinaryReadable<T, I> where I : IBinaryReadable<I>
            {
                ArgumentNullException.ThrowIfNull(ObjectReader);
                return T.Read(Reader, () => ObjectReader(Reader));
            }

            public T Read<T, I>(IBinaryReader<I>? ObjectReader = null) where T : IBinaryReadable<T, I> where I : IBinaryReadable<I>
            {
                ObjectReader ??= BinarySerializer.GetReader<I>();
                BinarySerializer.ReaderNotFound(ObjectReader);
                return T.Read(Reader, () => ObjectReader.Read(Reader));
            }


            public Index ReadIndex()
            {
                int Value = Reader.ReadInt32();
                return int.IsNegative(Value) ? new Index(-Value, true) : new Index(Value);
            }

            public Range ReadRange()
            {
                return new Range
                (
                    Reader.ReadIndex(),
                    Reader.ReadIndex()
                );
            }


            public TCollection ReadCollection<TCollection, T>() where TCollection : ICollection<T>, new() where T : IBinaryReadable<T>
            {
                return ReadCollection<TCollection, T>(Reader, static Count => new TCollection());
            }

            public TCollection ReadCollection<TCollection, T>(Func<int, TCollection> NewCollection) where TCollection : ICollection<T> where T : IBinaryReadable<T>
            {
                int Count = Reader.ReadInt32();
                TCollection Collection = NewCollection(Count);

                for (int i = 0; i < Count; i++)
                {
                    Collection.Add(T.Read(Reader));
                }

                return Collection;
            }


            public TCollection ReadCollection<TCollection, T>(IBinaryReader<T>? ObjectReader = null) where TCollection : ICollection<T>, new()
            {
                return ReadCollection(Reader, static Count => new TCollection(), ObjectReader);
            }

            public TCollection ReadCollection<TCollection, T>(Func<int, TCollection> NewCollection, IBinaryReader<T>? ObjectReader = null) where TCollection : ICollection<T>
            {
                ObjectReader ??= BinarySerializer.GetReader<T>();

                int Count = Reader.ReadInt32();
                TCollection Collection = NewCollection(Count);

                if (Count == 0)
                {
                    return Collection;
                }

                BinarySerializer.ReaderNotFound(ObjectReader);

                for (int i = 0; i < Count; i++)
                {
                    Collection.Add(ObjectReader.Read(Reader));
                }

                return Collection;
            }

            public TCollection ReadCollection<TCollection, T>(TCollection Collection, IBinaryReader<T>? ObjectReader = null) where TCollection : ICollection<T>
            {
                ObjectReader ??= BinarySerializer.GetReader<T>();

                int Count = Reader.ReadInt32();

                if (Count == 0)
                {
                    return Collection;
                }

                BinarySerializer.ReaderNotFound(ObjectReader);

                for (int i = 0; i < Count; i++)
                {
                    Collection.Add(ObjectReader.Read(Reader));
                }

                return Collection;
            }


            public TCollection ReadCollection<TCollection, T>(Func<T> Read) where TCollection : ICollection<T>, new()
            {
                return ReadCollection(Reader, static Count => new TCollection(), Read);
            }

            public TCollection ReadCollection<TCollection, T>(Func<int, TCollection> NewCollection, Func<T> Read) where TCollection : ICollection<T>
            {
                int Count = Reader.ReadInt32();
                TCollection Collection = NewCollection(Count);

                if (Count == 0)
                {
                    return Collection;
                }

                for (int i = 0; i < Count; i++)
                {
                    Collection.Add(Read());
                }

                return Collection;
            }

            public TCollection ReadCollection<TCollection, T>(TCollection Collection, Func<T> Read) where TCollection : ICollection<T>
            {
                int Count = Reader.ReadInt32();

                if (Count == 0)
                {
                    return Collection;
                }

                for (int i = 0; i < Count; i++)
                {
                    Collection.Add(Read());
                }

                return Collection;
            }


            public List<T> ReadList<T>() where T : IBinaryReadable<T>
            {
                return ReadCollection<List<T>, T>(Reader, static Count => new List<T>(Count));
            }

            public List<T> ReadList<T>(Func<T> Read)
            {
                return ReadCollection(Reader, static Count => new List<T>(Count), Read);
            }

            public List<T> ReadList<T>(IBinaryReader<T>? ObjectReader = null)
            {
                return ReadCollection(Reader, static Count => new List<T>(Count), ObjectReader);
            }


            public T[] ReadArray<T>() where T : IBinaryReadable<T>
            {
                int Count = Reader.ReadInt32();

                T[] Array = new T[Count];

                for (int i = 0; i < Count; i++)
                {
                    Array[i] = T.Read(Reader);
                }

                return Array;
            }

            public T[] ReadArray<T>(IBinaryReader<T>? ObjectReader = null)
            {
                ObjectReader ??= BinarySerializer.GetReader<T>();

                int Count = Reader.ReadInt32();
                T[] Array = new T[Count];

                if (Count == 0)
                {
                    return Array;
                }

                BinarySerializer.ReaderNotFound(ObjectReader);

                for (int i = 0; i < Count; i++)
                {
                    Array[i] = ObjectReader.Read(Reader);
                }

                return Array;
            }


            public bool[] ReadBooleanArray()
            {
                return ReadArray(Reader, bool.Serializer);
            }

            public byte[] ReadByteArray()
            {
                return ReadArray(Reader, byte.Serializer);
            }

            public sbyte[] ReadSByteArray()
            {
                return ReadArray(Reader, sbyte.Serializer);
            }

            public char[] ReadCharArray()
            {
                return ReadArray(Reader, char.Serializer);
            }

            public decimal[] ReadDecimalArray()
            {
                return ReadArray(Reader, decimal.Serializer);
            }

            public double[] ReadDoubleArray()
            {
                return ReadArray(Reader, double.Serializer);
            }

            public float[] ReadSingleArray()
            {
                return ReadArray(Reader, float.Serializer);
            }

            public int[] ReadInt32Array()
            {
                return ReadArray(Reader, int.Serializer);
            }

            public uint[] ReadUInt32Array()
            {
                return ReadArray(Reader, uint.Serializer);
            }

            public long[] ReadInt64Array()
            {
                return ReadArray(Reader, long.Serializer);
            }

            public ulong[] ReadUInt64Array()
            {
                return ReadArray(Reader, ulong.Serializer);
            }

            public short[] ReadInt16Array()
            {
                return ReadArray(Reader, short.Serializer);
            }

            public ushort[] ReadUInt16Array()
            {
                return ReadArray(Reader, ushort.Serializer);
            }

            public string[] ReadStringArray()
            {
                return ReadArray(Reader, string.Serializer);
            }


            public TCollection ReadBooleanCollection<TCollection>() where TCollection : ICollection<bool>, new()
            {
                return ReadCollection<TCollection, bool>(Reader, bool.Serializer);
            }

            public TCollection ReadByteCollection<TCollection>() where TCollection : ICollection<byte>, new()
            {
                return ReadCollection<TCollection, byte>(Reader, byte.Serializer);
            }

            public TCollection ReadSByteCollection<TCollection>() where TCollection : ICollection<sbyte>, new()
            {
                return ReadCollection<TCollection, sbyte>(Reader, sbyte.Serializer);
            }

            public TCollection ReadCharCollection<TCollection>() where TCollection : ICollection<char>, new()
            {
                return ReadCollection<TCollection, char>(Reader, char.Serializer);
            }

            public TCollection ReadDecimalCollection<TCollection>() where TCollection : ICollection<decimal>, new()
            {
                return ReadCollection<TCollection, decimal>(Reader, decimal.Serializer);
            }

            public TCollection ReadDoubleCollection<TCollection>() where TCollection : ICollection<double>, new()
            {
                return ReadCollection<TCollection, double>(Reader, double.Serializer);
            }

            public TCollection ReadSingleCollection<TCollection>() where TCollection : ICollection<float>, new()
            {
                return ReadCollection<TCollection, float>(Reader, float.Serializer);
            }

            public TCollection ReadInt32Collection<TCollection>() where TCollection : ICollection<int>, new()
            {
                return ReadCollection<TCollection, int>(Reader, int.Serializer);
            }

            public TCollection ReadUInt32Collection<TCollection>() where TCollection : ICollection<uint>, new()
            {
                return ReadCollection<TCollection, uint>(Reader, uint.Serializer);
            }

            public TCollection ReadInt64Collection<TCollection>() where TCollection : ICollection<long>, new()
            {
                return ReadCollection<TCollection, long>(Reader, long.Serializer);
            }

            public TCollection ReadUInt64Collection<TCollection>() where TCollection : ICollection<ulong>, new()
            {
                return ReadCollection<TCollection, ulong>(Reader, ulong.Serializer);
            }

            public TCollection ReadInt16Collection<TCollection>() where TCollection : ICollection<short>, new()
            {
                return ReadCollection<TCollection, short>(Reader, short.Serializer);
            }

            public TCollection ReadUInt16Collection<TCollection>() where TCollection : ICollection<ushort>, new()
            {
                return ReadCollection<TCollection, ushort>(Reader, ushort.Serializer);
            }

            public TCollection ReadStringCollection<TCollection>() where TCollection : ICollection<string>, new()
            {
                return ReadCollection<TCollection, string>(Reader, string.Serializer);
            }


            public TCollection ReadBooleanCollection<TCollection>(TCollection Collection) where TCollection : ICollection<bool>
            {
                return ReadCollection<TCollection, bool>(Reader, Collection, bool.Serializer);
            }

            public TCollection ReadByteCollection<TCollection>(TCollection Collection) where TCollection : ICollection<byte>
            {
                return ReadCollection<TCollection, byte>(Reader, Collection, byte.Serializer);
            }

            public TCollection ReadSByteCollection<TCollection>(TCollection Collection) where TCollection : ICollection<sbyte>
            {
                return ReadCollection<TCollection, sbyte>(Reader, Collection, sbyte.Serializer);
            }

            public TCollection ReadCharCollection<TCollection>(TCollection Collection) where TCollection : ICollection<char>
            {
                return ReadCollection<TCollection, char>(Reader, Collection, char.Serializer);
            }

            public TCollection ReadDecimalCollection<TCollection>(TCollection Collection) where TCollection : ICollection<decimal>
            {
                return ReadCollection<TCollection, decimal>(Reader, Collection, decimal.Serializer);
            }

            public TCollection ReadDoubleCollection<TCollection>(TCollection Collection) where TCollection : ICollection<double>
            {
                return ReadCollection<TCollection, double>(Reader, Collection, double.Serializer);
            }

            public TCollection ReadSingleCollection<TCollection>(TCollection Collection) where TCollection : ICollection<float>
            {
                return ReadCollection<TCollection, float>(Reader, Collection, float.Serializer);
            }

            public TCollection ReadInt32Collection<TCollection>(TCollection Collection) where TCollection : ICollection<int>
            {
                return ReadCollection<TCollection, int>(Reader, Collection, int.Serializer);
            }

            public TCollection ReadUInt32Collection<TCollection>(TCollection Collection) where TCollection : ICollection<uint>
            {
                return ReadCollection<TCollection, uint>(Reader, Collection, uint.Serializer);
            }

            public TCollection ReadInt64Collection<TCollection>(TCollection Collection) where TCollection : ICollection<long>
            {
                return ReadCollection<TCollection, long>(Reader, Collection, long.Serializer);
            }

            public TCollection ReadUInt64Collection<TCollection>(TCollection Collection) where TCollection : ICollection<ulong>
            {
                return ReadCollection<TCollection, ulong>(Reader, Collection, ulong.Serializer);
            }

            public TCollection ReadInt16Collection<TCollection>(TCollection Collection) where TCollection : ICollection<short>
            {
                return ReadCollection<TCollection, short>(Reader, Collection, short.Serializer);
            }

            public TCollection ReadUInt16Collection<TCollection>(TCollection Collection) where TCollection : ICollection<ushort>
            {
                return ReadCollection<TCollection, ushort>(Reader, Collection, ushort.Serializer);
            }

            public TCollection ReadStringCollection<TCollection>(TCollection Collection) where TCollection : ICollection<string>
            {
                return ReadCollection<TCollection, string>(Reader, Collection, string.Serializer);
            }
        }
    }
}