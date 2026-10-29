using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.ADF
{
    //_Primitives
    public abstract partial class BaseADFWriter
    {
        public void Write(string Name, bool Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Boolean, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, byte Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Byte, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, sbyte Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.SByte, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, short Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Int16, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, int Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Int32, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, long Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Int64, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, ushort Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.UInt16, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, uint Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.UInt32, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, ulong Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.UInt64, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, char Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Char, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, float Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Single, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, double Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Double, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, decimal Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Decimal, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, string Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.String, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, Half Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Half, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, Index Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Index, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, Range Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Range, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, BigInteger Value)
        {
            WritePrimitive<BigInteger>(Name, in Value, ADFPrimitives.BigInteger, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, RGBColor Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.RGB, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, RGBAColor Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.RGBA, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, Vector2 Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector2, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, Vector2Int Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector2Int, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, Vector3 Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector3, PrimitivesWriteStrategy.Instance);
        }

        public void Write(string Name, Vector3Int Value)
        {
            WritePrimitive(Name, in Value, ADFPrimitives.Vector3Int, PrimitivesWriteStrategy.Instance);
        }


        private void WritePrimitive<T>(string Name, in T Value, uint FormatId, IWriteStrategy<T> Strategy)
        {
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(Name.NotNull());
            var Target = GetWriteTarget(Name, in NameId, in FormatId);

            Strategy.Write(Context, Target, Value);
            OnWrited(Name, in NameId, in FormatId);
        }
    }
}