using System.Numerics;
using Zion.Geometry;
using Vector2 = Zion.Geometry.Vector2;
using Vector3 = Zion.Geometry.Vector3;

namespace Zion.ADF
{
    //_IEnumerable
    public abstract partial class BaseADFWriter
    {
        public void Write(string Name, IEnumerable<bool> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<byte> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<sbyte> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<short> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<int> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<long> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<ushort> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<uint> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<ulong> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<char> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<float> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<double> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<decimal> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<string> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<Half> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<Index> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<Range> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<BigInteger> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<RGBColor> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<RGBAColor> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<Vector2> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<Vector2Int> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<Vector3> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write(string Name, IEnumerable<Vector3Int> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }

        public void Write<T>(string Name, IEnumerable<T> Enumerable)
        {
            Write(Name, Enumerable.GetEnumerator());
        }


        public void Write(string Name, IEnumerator<bool> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<bool>
                (
                    Name, ADFPrimitives.Boolean,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<byte> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<byte>
                (
                    Name, ADFPrimitives.Byte,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<sbyte> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<sbyte>
                (
                    Name, ADFPrimitives.SByte,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<short> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<short>
                (
                    Name, ADFPrimitives.Int16,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<int> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<int>
                (
                    Name, ADFPrimitives.Int32,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<long> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<long>
                (
                    Name, ADFPrimitives.Int64,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<ushort> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<ushort>
                (
                    Name, ADFPrimitives.UInt16,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<uint> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<uint>
                (
                    Name, ADFPrimitives.UInt32,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<ulong> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<ulong>
                (
                    Name, ADFPrimitives.UInt64,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<char> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<char>
                (
                    Name, ADFPrimitives.Char,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<float> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<float>
                (
                    Name, ADFPrimitives.Single,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<double> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<double>
                (
                    Name, ADFPrimitives.Double,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<decimal> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<decimal>
                (
                    Name, ADFPrimitives.Decimal,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<string> Enumerator)
        {
            ThrowIfDisposed();

            var Compression = Context.Compression;
            var Registry = StringRegistry;

            var NameId = StringRegistry.GetOrAdd(Name);
            var Target = GetWriteTarget(Name, in NameId, ADFPrimitives.String | ADFPrimitives.Sequence);

            var Link = Reference.CreateNewReference(Target.ChildsLength);
            var Stream = Context.Arena.GetStream(64);
            var Count = 0;

            Target.BaseStream.WriteCompressed(Context, Link);
            Stream.SetPosition(4);

            while (Enumerator.MoveNext())
            {
                var Id = Registry.GetOrAdd(Enumerator.Current);

                if (Compression)
                {
                    Stream.Write7BitEncoded(Id);
                }
                else
                {
                    Stream.Write(Id);
                }

                Count++;
            }

            var Position = Stream.Position;

            Stream.SetPosition(0);
            Stream.Write(Count);
            Stream.SetPosition(Position);
            Target.Add(Stream);

            OnWrited(Name, in NameId, ADFPrimitives.String | ADFPrimitives.Sequence);
        }

        public void Write(string Name, IEnumerator<Half> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<Half>
                (
                    Name, ADFPrimitives.Half,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<Index> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<Index>
                (
                    Name, ADFPrimitives.Index,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<Range> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<Range>
                (
                    Name, ADFPrimitives.Range,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<BigInteger> Enumerator)
        {
            ThrowIfDisposed();

            var Compression = Context.Compression;
            var NameId = StringRegistry.GetOrAdd(Name);
            var Target = GetWriteTarget(Name, in NameId, ADFPrimitives.BigInteger | ADFPrimitives.Sequence);

            var Link = Reference.CreateNewReference(Target.ChildsLength);
            var Stream = Context.Arena.GetStream(64);
            var DataStream = Context.Arena.GetStream(64);
            var Count = 0;

            Target.BaseStream.WriteCompressed(Context, Link);
            Stream.SetPosition(4);

            while (Enumerator.MoveNext())
            {
                var Offset = Reference.CreateNewReference(DataStream.Length);

                if (Compression)
                {
                    Stream.Write7BitEncoded(Offset);
                }
                else
                {
                    Stream.Write(Offset);
                }
                DataStream.Write(Enumerator.Current);
                Count++;
            }

            var Position = Stream.Position;

            Stream.SetPosition(0);
            Stream.Write(Count);
            Stream.SetPosition(Position);
            Target.Add(new StreamGroup(Stream) { DataStream });

            OnWrited(Name, in NameId, ADFPrimitives.BigInteger | ADFPrimitives.Sequence);
        }

        public void Write(string Name, IEnumerator<RGBColor> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<RGBColor>
                (
                    Name, ADFPrimitives.RGB,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<RGBAColor> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<RGBAColor>
                (
                    Name, ADFPrimitives.RGBA,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<Vector2> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<Vector2>
                (
                    Name, ADFPrimitives.Vector2,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<Vector2Int> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<Vector2Int>
                (
                    Name, ADFPrimitives.Vector2Int,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<Vector3> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<Vector3>
                (
                    Name, ADFPrimitives.Vector3,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write(string Name, IEnumerator<Vector3Int> Enumerator)
        {
            WriteEnumerator
            (
                new EnumeratorWriteContext<Vector3Int>
                (
                    Name, ADFPrimitives.Vector3Int,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        public void Write<T>(string Name, IEnumerator<T> Enumerator)
        {
            //TODO: Write IEnumerator<Node>
        }


        private void WriteEnumerator<T>(EnumeratorWriteContext<T> WriteContext) where T : unmanaged
        {
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(WriteContext.Name);
            var Target = GetWriteTarget(WriteContext.Name, in NameId, in WriteContext.FormatId);

            WriteContext.Write(Context, Target);

            OnWrited(WriteContext.Name, in NameId, in WriteContext.FormatId);
        }
    }
}