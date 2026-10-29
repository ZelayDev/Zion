namespace Zion.ADF
{
    [Flags]
    internal enum WriteKind : byte
    {
        None   = 0b00,
        Auto   = 0b01,
        Manual = 0b10,
        Mixed  = 0b11
    }
}