namespace Zion.ADF
{
    [Flags]
    internal enum FormatFlags : ushort
    {
        None = 0,

        IsArray     = 0b00000001,
        IsReference = 0b00000010,
        IsAbstract  = 0b00000100,
        IsNullable  = 0b00001000,
        IsGenerated = 0b00010000,

        IsEnum      = 0b00100000,
        IsEnum8     = 0b00100000,
        IsEnum16    = 0b01100000,
        IsEnum32    = 0b10100000,
        IsEnum64    = 0b11100000,

        IsDeferred  = 0b100000000
    }
}