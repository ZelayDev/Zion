using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.ADF
{
    //_List
    public abstract partial class BaseADFWriter
    {
        public void Write(string Name, List<bool> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<byte> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<sbyte> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<short> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<int> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<long> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<ushort> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<uint> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<ulong> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<char> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<float> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<double> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<decimal> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<string> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Half> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Index> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Range> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<BigInteger> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<RGBColor> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<RGBAColor> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Vector2> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Vector2Int> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Vector3> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write(string Name, List<Vector3Int> List)
        {
            Write(Name, List, List.AsSpan());
        }

        public void Write<T>(string Name, List<T> List)
        {
            Write(Name, List, List.AsSpan());
        }


        public void Write(string Name, List<bool> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<byte> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<sbyte> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<short> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<int> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<long> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<ushort> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<uint> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<ulong> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<char> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<float> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<double> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<decimal> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<string> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<Half> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<Index> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<Range> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<BigInteger> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<RGBColor> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<RGBAColor> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<Vector2> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<Vector2Int> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<Vector3> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write(string Name, List<Vector3Int> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }

        public void Write<T>(string Name, List<T> List, int Start)
        {
            Write(Name, List.AsSpan(Start));
        }


        public void Write(string Name, List<bool> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<byte> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<sbyte> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<short> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<int> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<long> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<ushort> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<uint> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<ulong> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<char> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<float> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<double> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<decimal> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<string> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Half> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Index> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Range> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<BigInteger> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<RGBColor> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<RGBAColor> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Vector2> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Vector2Int> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Vector3> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write(string Name, List<Vector3Int> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }

        public void Write<T>(string Name, List<T> List, int Start, int Count)
        {
            Write(Name, List.AsSpan(Start, Count));
        }
    }
}