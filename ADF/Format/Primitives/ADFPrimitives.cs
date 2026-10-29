using Zion.Vectors;
using BigInteger = System.Numerics.BigInteger;

namespace Zion.ADF
{
    internal static class ADFPrimitives
    {
        #region Info
        private static readonly PrimitiveInfo[] PrimitivesInfo =
        [
            new(typeof(bool),       sizeof(bool)    ),
            new(typeof(byte),       sizeof(byte)    ),
            new(typeof(sbyte),      sizeof(sbyte)   ),
            new(typeof(short),      sizeof(short)   ),
            new(typeof(int),        sizeof(int)     ),
            new(typeof(long),       sizeof(long)    ),
            new(typeof(ushort),     sizeof(ushort)  ),
            new(typeof(uint),       sizeof(uint)    ),
            new(typeof(ulong),      sizeof(ulong)   ),
            new(typeof(char),       sizeof(char)    ),
            new(typeof(float),      sizeof(float)   ),
            new(typeof(double),     sizeof(double)  ),
            new(typeof(decimal),    sizeof(decimal) ),
            new(typeof(string),     4               ),
            new(typeof(Half),       2               ),
            new(typeof(Index),      4               ),
            new(typeof(Range),      8               ),
            new(typeof(BigInteger), 8               ),
            new(typeof(RGBColor),   3               ),
            new(typeof(RGBAColor),  4               ),
            new(typeof(Vector2),    8               ),
            new(typeof(Vector2Int), 8               ),
            new(typeof(Vector3),    12              ),
            new(typeof(Vector3Int), 12              )
        ];

        #endregion

        #region Constants
        public const int Count = 128;

        public const uint Sequence = 1u << 31;

        public const uint Boolean = 0;
        public const uint Byte    = 1;
        public const uint SByte   = 2;

        public const uint Int16   = 3;
        public const uint Int32   = 4;
        public const uint Int64   = 5;
        public const uint UInt16  = 6;
        public const uint UInt32  = 7;
        public const uint UInt64  = 8;

        public const uint Char    = 9;
        public const uint Single  = 10;
        public const uint Double  = 11;
        public const uint Decimal = 12;
        public const uint String  = 13;

        public const uint Half       = 14;
        public const uint Index      = 15;
        public const uint Range      = 16;
        public const uint BigInteger = 17;

        public const uint RGB  = 18;
        public const uint RGBA = 19;

        public const uint Vector2    = 20;
        public const uint Vector2Int = 21;
        public const uint Vector3    = 22;
        public const uint Vector3Int = 23;

        public const uint RawString = 24;

        public const uint Object = 127;

        #endregion

        #region PublicMethods
        public static PrimitiveInfo GetInfo(uint Id)
        {
            return PrimitivesInfo[Id];
        }

        public static bool TryGetInfo(uint Id, out PrimitiveInfo Info)
        {
            if (Id < PrimitivesInfo.Length)
            {
                Info = PrimitivesInfo[Id];
                return true;
            }
            Info = default;
            return false;
        }

        public static ReadOnlySpan<PrimitiveInfo> GetPrimitivesInfo()
        {
            return PrimitivesInfo;
        }

        public static void AddTo(Dictionary<Type, WriteEntry> Strategies)
        {
            var Primitives = GetPrimitivesInfo();
            var Strategy = PrimitivesWriteStrategy.Instance;

            for (int i = 0; i < Primitives.Length; i++)
            {
                var Info = Primitives[i];
                Strategies.Add(Info.Type, new((uint)i, Strategy));
            }
        }

        #endregion
    }
}