namespace Zion.ADF
{
    internal static class ArenaStreamExtensions
    {
        extension(ArenaStream Stream)
        {
            public void WriteCompressedZero(ADFWritingContext Context)
            {
                if (Context.Compression)
                {
                    Stream.Write((byte)0);
                }
                else
                {
                    Stream.Write(0u);
                }
            }

            public void WriteCompressed(ADFWritingContext Context, uint Value)
            {
                if (Context.Compression)
                {
                    Stream.Write7BitEncoded(Value);
                }
                else
                {
                    Stream.Write(Value);
                }
            }

            public void WriteCompressed(ADFWritingContext Context, ulong Value)
            {
                if (Context.Compression)
                {
                    Stream.Write7BitEncoded(Value);
                }
                else
                {
                    Stream.Write(Value);
                }
            }
        }
    }
}