using System.Numerics;
using Zion.Geometry;

namespace Zion.ADF
{
    internal sealed class PrimitivesWriteStrategy
        : IWriteStrategy<bool>,
          IWriteStrategy<byte>,
          IWriteStrategy<sbyte>,
          IWriteStrategy<short>,
          IWriteStrategy<int>,
          IWriteStrategy<long>,
          IWriteStrategy<ushort>,
          IWriteStrategy<uint>,
          IWriteStrategy<ulong>,
          IWriteStrategy<char>,
          IWriteStrategy<float>,
          IWriteStrategy<double>,
          IWriteStrategy<decimal>,
          IWriteStrategy<string>,
          IWriteStrategy<Half>,
          IWriteStrategy<Index>,
          IWriteStrategy<Range>,
          IWriteStrategy<BigInteger>,
          IWriteStrategy<RGBColor>,
          IWriteStrategy<RGBAColor>,
          IWriteStrategy<Vector2Int>,
          IWriteStrategy<Vector3Int>,
          IWriteStrategy<Vectors.Vector2>,
          IWriteStrategy<Vectors.Vector3>,
          IWriteStrategy<System.Numerics.Vector2>,
          IWriteStrategy<System.Numerics.Vector3>
    {
        public static readonly PrimitivesWriteStrategy Instance = new();


        public void WriteData(ADFWritingContext Context, StreamGroup Target, bool Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, byte Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, sbyte Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, short Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, int Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncoded(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, long Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncoded(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, ushort Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, uint Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncoded(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, ulong Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncoded(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, char Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, float Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, double Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, decimal Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, string Value)
        {
            var Id = Context.Registries.StringRegistry.GetOrAdd(Value);

            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncoded(Id);
            }
            else
            {
                Target.BaseStream.Write(Id);
            }
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Half Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Index Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncoded(Value);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Range Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncoded(Value.Start);
                Target.BaseStream.Write7BitEncoded(Value.End);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, BigInteger Value)
        {
            var BaseStream = Target.BaseStream;
            var Stream = Context.Arena.GetStream(1);
            var Link = Reference.CreateNewReference(Target.ChildsLength);

            Stream.Write(Value);
            Target.Add(Stream);

            Target.BaseStream.WriteCompressed(Context, Link);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, RGBColor Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, RGBAColor Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Vector2Int Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncoded(Value.X);
                Target.BaseStream.Write7BitEncoded(Value.Y);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Vector3Int Value)
        {
            if (Context.Compression)
            {
                Target.BaseStream.Write7BitEncoded(Value.X);
                Target.BaseStream.Write7BitEncoded(Value.Y);
                Target.BaseStream.Write7BitEncoded(Value.Z);
            }
            else
            {
                Target.BaseStream.Write(Value);
            }
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Vectors.Vector2 Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, Vectors.Vector3 Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, System.Numerics.Vector2 Value)
        {
            Target.BaseStream.Write(Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, System.Numerics.Vector3 Value)
        {
            Target.BaseStream.Write(Value);
        }
    }
}