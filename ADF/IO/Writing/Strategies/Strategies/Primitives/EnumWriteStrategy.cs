using System.Runtime.CompilerServices;

namespace Zion.ADF
{
    internal sealed class EnumWriteStrategy<T> : IWriteStrategy<T>
    {
        public EnumWriteStrategy()
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidCastException($"{typeof(T)} is not Enum");
            }
        }


        public void WriteData(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            Span<byte> Buffer = stackalloc byte[Unsafe.SizeOf<T>()];
            Unsafe.WriteUnaligned(ref Buffer[0], Value);
            Target.BaseStream.Write(Buffer);
        }
    }
}