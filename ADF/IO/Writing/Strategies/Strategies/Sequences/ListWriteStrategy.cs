using System.Numerics;
using Zion.Vectors;

namespace Zion.ADF
{
    internal sealed class ListWriteStrategy
        : IWriteStrategy<List<bool>>,
          IWriteStrategy<List<byte>>,
          IWriteStrategy<List<sbyte>>,
          IWriteStrategy<List<short>>,
          IWriteStrategy<List<int>>,
          IWriteStrategy<List<long>>,
          IWriteStrategy<List<ushort>>,
          IWriteStrategy<List<uint>>,
          IWriteStrategy<List<ulong>>,
          IWriteStrategy<List<char>>,
          IWriteStrategy<List<float>>,
          IWriteStrategy<List<double>>,
          IWriteStrategy<List<decimal>>,
          IWriteStrategy<List<string>>,
          IWriteStrategy<List<Half>>,
          IWriteStrategy<List<Index>>,
          IWriteStrategy<List<Range>>,
          IWriteStrategy<List<BigInteger>>,
          IWriteStrategy<List<RGBColor>>,
          IWriteStrategy<List<RGBAColor>>,
          IWriteStrategy<List<Vectors.Vector2>>,
          IWriteStrategy<List<Vector2Int>>,
          IWriteStrategy<List<Vectors.Vector3>>,
          IWriteStrategy<List<Vector3Int>>
    {
        public static readonly ListWriteStrategy Instance = new();


        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<bool> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<byte> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<sbyte> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<short> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<int> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<long> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<ushort> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<uint> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<ulong> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<char> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<float> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<double> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<decimal> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<string> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<Half> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<Index> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<Range> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<BigInteger> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<RGBColor> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<RGBAColor> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<Vectors.Vector2> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<Vector2Int> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<Vectors.Vector3> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, List<Vector3Int> Value)
        {
            SequenceWriteHelper.Write(Context, Target, Value, Value.AsSpan());
        }
    }
}