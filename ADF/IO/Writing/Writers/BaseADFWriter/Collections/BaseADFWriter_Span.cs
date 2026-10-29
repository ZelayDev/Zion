using System.Numerics;
using System.Xml.Linq;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.ADF
{
    //_Span
    public abstract partial class BaseADFWriter
    {
        public void Write(string Name, ReadOnlySpan<bool> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Boolean, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<byte> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Byte, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<sbyte> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.SByte, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<short> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Int16, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<int> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Int32, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<long> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Int64, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<ushort> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.UInt16, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<uint> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.UInt32, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<ulong> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.UInt64, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<char> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Char, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<float> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Single, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<double> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Double, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<decimal> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Decimal, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<string> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.String, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<Half> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Half, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<Index> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Index, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<Range> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Range, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<BigInteger> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.BigInteger, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<RGBColor> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.RGB, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<RGBAColor> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.RGBA, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<Vector2> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Vector2, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<Vector2Int> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Vector2Int, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<Vector3> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Vector3, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write(string Name, ReadOnlySpan<Vector3Int> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Vector3Int, null,
                Span, SequenceWriteHelper.Write
            );
        }

        public void Write<T>(string Name, ReadOnlySpan<T> Span)
        {
            Write(Name, null, Span);
        }


        private void Write(string Name, object? Collection, ReadOnlySpan<bool> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Boolean, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<byte> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Byte, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<sbyte> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.SByte, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<short> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Int16, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<int> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Int32, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<long> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Int64, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<ushort> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.UInt16, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<uint> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.UInt32, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<ulong> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.UInt64, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<char> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Char, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<float> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Single, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<double> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Double, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<decimal> Span) 
        {
            WriteSpan
            (
                Name, ADFPrimitives.Decimal, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<string> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.String, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Half> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Half, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Index> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Index, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Range> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Range, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<BigInteger> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.BigInteger, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<RGBColor> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.RGB, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<RGBAColor> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.RGBA, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector2> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Vector2, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector2Int> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Vector2Int, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector3> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Vector3, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write(string Name, object? Collection, ReadOnlySpan<Vector3Int> Span)
        {
            WriteSpan
            (
                Name, ADFPrimitives.Vector3Int, Collection,
                Span, SequenceWriteHelper.Write
            );
        }

        private void Write<T>(string Name, object? Collection, ReadOnlySpan<T> Span)
        {
            var FormatId = TypeAssociation.GetOrAddDeferred(typeof(T), FormatRegistry);

            WriteSpan
            (
                Name, FormatId, Collection,
                Span, SequenceWriteHelper.Write
            );
        }


        private void WriteSpan<T>(string Name, uint FormatId, object? Collection, ReadOnlySpan<T> Span, SequenceWriteAction<T> WriteAction)
        {
            ThrowIfDisposed();

            var Context = this.Context;
            var NameId  = Context.Registries.StringRegistry.GetOrAdd(Name);
            var Target  = GetWriteTarget(Name, in NameId, in FormatId);

            WriteAction(Context, Target, Collection, Span);

            OnWrited(Name, in NameId, in FormatId);
        }
    }
}