using System.Numerics;

namespace Zion.STP
{
    public readonly record struct NumberParsingParameters<T>(
        bool              OnlyPositive,
        bool              HasLimits,
        int               BitCount,
        T                 MinValue,
        T                 MaxValue,
        Func<T, int, T>   Sum,
        Func<T, int, T>   Multiply,
        Func<T, int, T>   Divide,
        Func<T, int, int> Remainder,
        Func<T, int, T>   LeftShift,
        params string[]   Suffixes
    ) where T : IComparable<T>, INumber<T>;

    public static class NumberParsingParameters
    {
        public static readonly NumberParsingParameters<byte> Byte = new
        (
            true, true, 8, System.Byte.MinValue, System.Byte.MaxValue,
            (A, B) => (byte)(A + B),
            (A, B) => (byte)(A * B),
            (A, B) => (byte)(A / B),
            (A, B) => (byte)(A % B),
            (A, B) => (byte)(A << B)
        );

        public static readonly NumberParsingParameters<sbyte> SByte = new
        (
            false, true, 7, System.SByte.MinValue, System.SByte.MaxValue,
            (A, B) => (sbyte)(A + B),
            (A, B) => (sbyte)(A * B),
            (A, B) => (sbyte)(A / B),
            (A, B) => (sbyte)(A % B),
            (A, B) => (sbyte)(A << B),
            "sb", "SB"
        );

        public static readonly NumberParsingParameters<short> Int16 = new
        (
            false, true, 15, System.Int16.MinValue, System.Int16.MaxValue,
            (A, B) => (short)(A + B),
            (A, B) => (short)(A * B),
            (A, B) => (short)(A / B),
            (A, B) => (short)(A % B),
            (A, B) => (short)(A << B),
            "s", "S"
        );

        public static readonly NumberParsingParameters<int> Int32 = new
        (
            false, true, 31, System.Int32.MinValue, System.Int32.MaxValue,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => A % B,
            (A, B) => A << B
        );

        public static readonly NumberParsingParameters<long> Int64 = new
        (
            false, true, 63, System.Int64.MinValue, System.Int64.MaxValue,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            "l", "L"
        );

        public static readonly NumberParsingParameters<Int128> Int128 = new
        (
            false, true, 127, Int128.MinValue, Int128.MaxValue,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            "q", "Q"
        );

        public static readonly NumberParsingParameters<ushort> UInt16 = new
        (
            true, true, 16, System.UInt16.MinValue, System.UInt16.MaxValue,
            (A, B) => (ushort)(A + B),
            (A, B) => (ushort)(A * B),
            (A, B) => (ushort)(A / B),
            (A, B) => (ushort)(A % B),
            (A, B) => (ushort)(A << B),
            "us", "US"
        );

        public static readonly NumberParsingParameters<uint> UInt32 = new
        (
            true, true, 32, System.UInt32.MinValue, System.UInt32.MaxValue,
            (A, B) => (uint)(A + B),
            (A, B) => (uint)(A * B),
            (A, B) => (uint)(A / B),
            (A, B) => (int)(A % B),
            (A, B) => A << B,
            "u", "U"
        );

        public static readonly NumberParsingParameters<ulong> UInt64 = new
        (
            true, true, 64, System.UInt64.MinValue, System.UInt64.MaxValue,
            (A, B) => A + (ulong)B,
            (A, B) => A * (ulong)B,
            (A, B) => A / (ulong)B,
            (A, B) => (int)(A % (ulong)B),
            (A, B) => A << B,
            "ul", "UL"
        );

        public static readonly NumberParsingParameters<UInt128> UInt128 = new
        (
            true, true, 128, UInt128.MinValue, UInt128.MaxValue,
            (A, B) => A + (UInt128)B,
            (A, B) => A * (UInt128)B,
            (A, B) => A / (UInt128)B,
            (A, B) => (int)(A % (UInt128)B),
            (A, B) => A << B,
            "uq", "UQ"
        );

        public static readonly NumberParsingParameters<BigInteger> BigInt = new
        (
            false, false, 0, BigInteger.Zero, BigInteger.Zero,
            (A, B) => A + B,
            (A, B) => A * B,
            (A, B) => A / B,
            (A, B) => (int)(A % B),
            (A, B) => A << B
        );
    }
}