using System.Numerics;
using Zion.Vectors;

namespace Zion.ADF
{
    internal sealed class ArrayWriteStrategy
        : IWriteStrategy<bool[]>,
          IWriteStrategy<byte[]>,
          IWriteStrategy<sbyte[]>,
          IWriteStrategy<short[]>,
          IWriteStrategy<int[]>,
          IWriteStrategy<long[]>,
          IWriteStrategy<ushort[]>,
          IWriteStrategy<uint[]>,
          IWriteStrategy<ulong[]>,
          IWriteStrategy<char[]>,
          IWriteStrategy<float[]>,
          IWriteStrategy<double[]>,
          IWriteStrategy<decimal[]>,
          IWriteStrategy<string[]>,
          IWriteStrategy<Half[]>,
          IWriteStrategy<Index[]>,
          IWriteStrategy<Range[]>,
          IWriteStrategy<BigInteger[]>,
          IWriteStrategy<RGBColor[]>,
          IWriteStrategy<RGBAColor[]>,
          IWriteStrategy<Vectors.Vector2[]>,
          IWriteStrategy<Vector2Int[]>,
          IWriteStrategy<Vectors.Vector3[]>,
          IWriteStrategy<Vector3Int[]>
    {
        public static readonly ArrayWriteStrategy Instance = new();


        public void WriteData(ADFWritingContext Context, StreamGroup Target, bool[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, byte[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, sbyte[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, short[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, int[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, long[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, ushort[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, uint[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, ulong[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, char[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, float[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, double[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, decimal[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, string[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Half[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Index[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Range[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, BigInteger[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, RGBColor[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, RGBAColor[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Vectors.Vector2[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Vector2Int[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Vectors.Vector3[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Vector3Int[] Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }
    }
}