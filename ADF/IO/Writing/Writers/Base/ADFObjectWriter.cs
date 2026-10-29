namespace Zion.ADF
{
    public abstract class ADFObjectWriter : BaseADFWriter
    {
        internal ADFObjectWriter(ADFWritingContext Context, StreamGroup Target)
            : base(Context, Target) { }
    }
}