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
            Target.BaseStream.UseSpan
            (
                Unsafe.SizeOf<T>(),
                Span => Unsafe.WriteUnaligned(ref Span[0], Value)
            );
        }
    }
}