using Zion.Serialization;

namespace Zion.ADF
{
    public readonly struct ADFHeader : IBinarySerializable<ADFHeader>
    {
        public bool Compression { get; init; }
        
        
        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Compression);
        }

        public static ADFHeader Read(BinaryReader Reader)
        {
            return new()
            {
                Compression = Reader.ReadBoolean()
            };
        }
    }
}