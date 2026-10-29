namespace Zion
{
    public static class EnumExtensions
    {
        extension(Enum Value)
        {
            public static int SizeOf(Type EnumType)
            {
                return Type.GetTypeCode(Enum.GetUnderlyingType(EnumType)) switch
                {
                    TypeCode.Byte or TypeCode.SByte => 1,
                    TypeCode.Int16 or TypeCode.UInt16 => 2,
                    TypeCode.Int32 or TypeCode.UInt32 => 4,
                    TypeCode.Int64 or TypeCode.UInt64 => 8,
                    _ => throw new InvalidOperationException($"{EnumType} is not Enum")
                };
            }
        }
    }
}