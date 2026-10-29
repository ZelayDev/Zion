using System.Reflection;
using System.Runtime.CompilerServices;

namespace Zion.ADF
{
    internal static class AutoSerializerHelper
    {
        private static readonly MethodInfo WriteEnumMethod = typeof(AutoSerializerHelper).GetMethod(nameof(WriteEnum))!;
        private static readonly MethodInfo WriteObjectMethod = typeof(AutoSerializerHelper).GetMethod(nameof(Write))!;


        public static MethodInfo GetWriter(Type Type)
        {
            return Type.IsEnum
                ? GetEnumWriter(Type)
                : GetObjectWriter(Type);
        }

        public static MethodInfo GetEnumWriter(Type Type)
        {
            return WriteEnumMethod.MakeGenericMethod(Type);
        }

        public static MethodInfo GetObjectWriter(Type Type)
        {
            return WriteObjectMethod.MakeGenericMethod(Type);
        }


        private static void WriteEnum<T>(ADFWritingContext Context, StreamGroup Target, T Value) where T : struct, Enum
        {
            Target.BaseStream.UseSpan
            (
                Unsafe.SizeOf<T>(),
                span => Unsafe.WriteUnaligned(ref span[0], Value)
            );
        }

        private static void Write<T>(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            var Entry = Context.WriteStrategies.GetEntry<T>(Context, Value!.GetType());
            Entry.Strategy.WriteData(Context, Target, Value);
        }
    }
}