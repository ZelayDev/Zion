using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

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


        private void Write(string Name, IEnumerator<bool> Enumerator)
        {
            WriteEnumerator<bool>
            (
                new
                (
                    Name, ADFPrimitives.Boolean,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<byte> Enumerator)
        {
            WriteEnumerator<byte>
            (
                new
                (
                    Name, ADFPrimitives.Byte,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<sbyte> Enumerator)
        {
            WriteEnumerator<sbyte>
            (
                new
                (
                    Name, ADFPrimitives.SByte,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<short> Enumerator)
        {
            WriteEnumerator<short>
            (
                new
                (
                    Name, ADFPrimitives.Int16,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<int> Enumerator)
        {
            WriteEnumerator<int>
            (
                new
                (
                    Name, ADFPrimitives.Int32,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<long> Enumerator)
        {
            WriteEnumerator<long>
            (
                new
                (
                    Name, ADFPrimitives.Int64,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<ushort> Enumerator)
        {
            WriteEnumerator<ushort>
            (
                new
                (
                    Name, ADFPrimitives.UInt16,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<uint> Enumerator)
        {
            WriteEnumerator<uint>
            (
                new
                (
                    Name, ADFPrimitives.UInt32,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<ulong> Enumerator)
        {
            WriteEnumerator<ulong>
            (
                new
                (
                    Name, ADFPrimitives.UInt64,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<char> Enumerator)
        {
            WriteEnumerator<char>
            (
                new
                (
                    Name, ADFPrimitives.Char,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<float> Enumerator)
        {
            WriteEnumerator<float>
            (
                new
                (
                    Name, ADFPrimitives.Single,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<double> Enumerator)
        {
            WriteEnumerator<double>
            (
                new
                (
                    Name, ADFPrimitives.Double,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<decimal> Enumerator)
        {
            WriteEnumerator<decimal>
            (
                new
                (
                    Name, ADFPrimitives.Decimal,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        //!!!
        private void Write(string Name, IEnumerator<string> Enumerator)
        {
            //TODO: Вынести общую логику
            ThrowIfDisposed();

            var Registry = StringRegistry;

            var NameId = StringRegistry.GetOrAdd(Name);
            var Target = GetWriteTarget(Name, in NameId, ADFPrimitives.String | ADFPrimitives.Sequence);
            var Stream = Target.BaseStream;

            if (Context.Compression)
            {
                while (Enumerator.MoveNext())
                {
                    var Id = Registry.GetOrAdd(Enumerator.Current);
                    Stream.Write7BitEncoded(Id);
                }
            }
            else
            {
                while (Enumerator.MoveNext())
                {
                    var Id = Registry.GetOrAdd(Enumerator.Current);
                    Stream.Write(Id);
                }
            }

            OnWrited(Name, in NameId, ADFPrimitives.String | ADFPrimitives.Sequence);
        }

        private void Write(string Name, IEnumerator<Half> Enumerator)
        {
            WriteEnumerator<Half>
            (
                new
                (
                    Name, ADFPrimitives.Half,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<Index> Enumerator)
        {
            WriteEnumerator<Index>
            (
                new
                (
                    Name, ADFPrimitives.Index,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<Range> Enumerator)
        {
            WriteEnumerator<Range>
            (
                new
                (
                    Name, ADFPrimitives.Range,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        //!!!
        private void Write(string Name, IEnumerator<BigInteger> Enumerator)
        {
            //TODO: Вынести общую логику
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(Name);
            var Target = GetWriteTarget(Name, in NameId, ADFPrimitives.BigInteger | ADFPrimitives.Sequence);

            var Base   = Target.BaseStream;
            var Stream = Context.Arena.GetStream(64);
            var Offset = Target.ChildsLength;

            while (Enumerator.MoveNext())
            {
                Base.Write(Reference.CreateNewReference(Offset + Stream.Length));
                Stream.Write(Enumerator.Current);
            }

            Target.Add(Stream);

            OnWrited(Name, in NameId, ADFPrimitives.BigInteger | ADFPrimitives.Sequence);
        }

        private void Write(string Name, IEnumerator<RGBColor> Enumerator)
        {
            WriteEnumerator<RGBColor>
            (
                new
                (
                    Name, ADFPrimitives.RGB,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<RGBAColor> Enumerator)
        {
            WriteEnumerator<RGBAColor>
            (
                new
                (
                    Name, ADFPrimitives.RGBA,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<Vector2> Enumerator)
        {
            WriteEnumerator<Vector2>
            (
                new
                (
                    Name, ADFPrimitives.Vector2,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<Vector2Int> Enumerator)
        {
            WriteEnumerator<Vector2Int>
            (
                new
                (
                    Name, ADFPrimitives.Vector2Int,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<Vector3> Enumerator)
        {
            WriteEnumerator<Vector3>
            (
                new
                (
                    Name, ADFPrimitives.Vector3,
                    Enumerator, SequenceWriteHelper.Write
                )
            );
        }

        private void Write(string Name, IEnumerator<Vector3Int> Enumerator)
        {
            WriteEnumerator<Vector3Int>
            (
                new
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


        //TODO: Переделать запись под SequenceWriteHelper.Write(Context, Target...)
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