using System.Numerics;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.ADF
{
    //_Array
    public abstract partial class BaseADFWriter
    {
        public void Write(string Name, bool[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, byte[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, sbyte[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, short[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, int[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, long[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, ushort[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, uint[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, ulong[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, char[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, float[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, double[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, decimal[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, string[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Half[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Index[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Range[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, BigInteger[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, RGBColor[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, RGBAColor[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Vector2[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Vector2Int[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Vector3[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write(string Name, Vector3Int[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }

        public void Write<T>(string Name, T[] Array)
        {
            Write(Name, Array, Array.AsSpan());
        }


        public void Write(string Name, bool[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, byte[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, sbyte[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, short[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, int[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, long[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, ushort[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, uint[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, ulong[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, char[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, float[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, double[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, decimal[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, string[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, Half[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, Index[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, Range[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, BigInteger[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, RGBColor[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, RGBAColor[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, Vector2[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, Vector2Int[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, Vector3[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write(string Name, Vector3Int[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }

        public void Write<T>(string Name, T[] Array, int Start)
        {
            Write(Name, Array.AsSpan(Start));
        }


        public void Write(string Name, bool[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, byte[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, sbyte[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, short[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, int[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, long[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, ushort[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, uint[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, ulong[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, char[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, float[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, double[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, decimal[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, string[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Half[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Index[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Range[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, BigInteger[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, RGBColor[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, RGBAColor[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Vector2[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Vector2Int[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Vector3[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write(string Name, Vector3Int[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }

        public void Write<T>(string Name, T[] Array, int Start, int Count)
        {
            Write(Name, Array.AsSpan(Start, Count));
        }
    }
}