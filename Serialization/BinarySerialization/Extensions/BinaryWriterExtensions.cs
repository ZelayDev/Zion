namespace Zion.Serialization
{
    public static class BinaryWriterExtensions
    {
        extension(BinaryWriter Writer)
        {
            public void Write<T>(T Value) where T : IBinaryWritable
            {
                Value.Write(Writer);
            }

            public void Write<T>(T Value, IBinaryWriter<T>? ObjectWriter = null)
            {
                ObjectWriter ??= BinarySerializer.GetWriter<T>();
                BinarySerializer.WriterNotFound(ObjectWriter);
                ObjectWriter.Write(Writer, Value);
            }


            public void Write<T, I>(T Value) where T : IBinaryWritable<I> where I : IBinaryWritable
            {
                Value.Write(Writer, Item => Item.Write(Writer));
            }

            public void Write<T, I>(T Value, Action<BinaryWriter, I> ObjectWriter) where T : IBinaryWritable<I>
            {
                ArgumentNullException.ThrowIfNull(ObjectWriter);
                Value.Write(Writer, Item => ObjectWriter(Writer, Item));
            }

            public void Write<T, I>(T Value, IBinaryWriter<I>? ObjectWriter = null) where T : IBinaryWritable<I>
            {
                ObjectWriter ??= BinarySerializer.GetWriter<I>();
                BinarySerializer.WriterNotFound(ObjectWriter);
                Value.Write(Writer, Item => ObjectWriter.Write(Writer, Item));
            }


            public void Write(Index Value)
            {
                Writer.Write(Value.IsFromEnd ? -Value.Value : Value.Value);
            }

            public void Write(Range Value)
            {
                Writer.Write(Value.Start);
                Writer.Write(Value.End);
            }


            public void WriteCollection<T>(ICollection<T> Collection) where T : IBinaryWritable
            {
                ArgumentNullException.ThrowIfNull(Collection);

                Writer.Write(Collection.Count);
                foreach (T Item in Collection)
                {
                    Writer.Write(Item);
                }
            }

            public void WriteCollection<T>(ICollection<T> Collection, Action<T> ObjectWriter)
            {
                ArgumentNullException.ThrowIfNull(Collection);
                ArgumentNullException.ThrowIfNull(ObjectWriter);

                Writer.Write(Collection);
                foreach (T Item in Collection)
                {
                    ObjectWriter(Item);
                }
            }

            public void WriteCollection<T>(ICollection<T> Collection, IBinaryWriter<T>? ObjectWriter = null)
            {
                ArgumentNullException.ThrowIfNull(Collection);

                if (Collection.Count == 0)
                {
                    Writer.Write(0);
                    return;
                }

                ObjectWriter ??= BinarySerializer.GetWriter<T>();

                if (ObjectWriter is not null)
                {
                    Writer.Write(Collection.Count);
                    foreach (T Item in Collection)
                    {
                        ObjectWriter.Write(Writer, Item);
                    }
                }
                else if (Collection.First() is IBinaryWritable)
                {
                    Writer.Write(Collection.Count);
                    foreach (IBinaryWritable Item in Collection.Cast<IBinaryWritable>())
                    {
                        Item.Write(Writer);
                    }
                }
                else
                {
                    BinarySerializer.WriterNotFound<T>();
                }
            }


            public void WriteCollection(ICollection<bool> Collection)
            {
                WriteCollection(Writer, Collection, bool.Serializer);
            }

            public void WriteCollection(ICollection<byte> Collection)
            {
                WriteCollection(Writer, Collection, byte.Serializer);
            }

            public void WriteCollection(ICollection<sbyte> Collection)
            {
                WriteCollection(Writer, Collection, sbyte.Serializer);
            }

            public void WriteCollection(ICollection<char> Collection)
            {
                WriteCollection(Writer, Collection, char.Serializer);
            }

            public void WriteCollection(ICollection<decimal> Collection)
            {
                WriteCollection(Writer, Collection, decimal.Serializer);
            }

            public void WriteCollection(ICollection<double> Collection)
            {
                WriteCollection(Writer, Collection, double.Serializer);
            }

            public void WriteCollection(ICollection<float> Collection)
            {
                WriteCollection(Writer, Collection, float.Serializer);
            }

            public void WriteCollection(ICollection<int> Collection)
            {
                WriteCollection(Writer, Collection, int.Serializer);
            }

            public void WriteCollection(ICollection<uint> Collection)
            {
                WriteCollection(Writer, Collection, uint.Serializer);
            }

            public void WriteCollection(ICollection<long> Collection)
            {
                WriteCollection(Writer, Collection, long.Serializer);
            }

            public void WriteCollection(ICollection<ulong> Collection)
            {
                WriteCollection(Writer, Collection, ulong.Serializer);
            }

            public void WriteCollection(ICollection<short> Collection)
            {
                WriteCollection(Writer, Collection, short.Serializer);
            }

            public void WriteCollection(ICollection<ushort> Collection)
            {
                WriteCollection(Writer, Collection, ushort.Serializer);
            }

            public void WriteCollection(ICollection<string> Collection)
            {
                WriteCollection(Writer, Collection, string.Serializer);
            }
        }
    }
}