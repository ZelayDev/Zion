using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Zion.ADF
{
    internal static class AutoSerializerBuilder<T>
    {
        private static readonly Dictionary<Type, MethodInfo> WriteMethodCache = new();

        private const BindingFlags Flags = BindingFlags.Public
                                         | BindingFlags.NonPublic
                                         | BindingFlags.Instance
                                         | BindingFlags.DeclaredOnly;

        public static AutoType<T> Create(ADFWritingContext WritingContext, Type Type)
        {
            CheckContext(WritingContext, Type);

            var Fields = Type.GetFields(Flags);
            var Context = new AutoSerializerBuilderContext<T>(Fields.Length);
            var Writer = Expression.Lambda<AutoWriter<T>>
            (
                Expression.Block(GetWriters(Fields, Context)),
                Context.TargetParameter,
                Context.ValueParameter
            ).Compile();

            var Schema = new TypeSchema(Type, Context.GetFields());

            return new(Writer, Schema);
        }


        private static IEnumerable<Expression> GetWriters(FieldInfo[] Fields, AutoSerializerBuilderContext<T> Context)
        {
            foreach (var Pair in FilterFields(Fields))
            {
                var Info = Pair.Item1;
                var Name = Pair.Item2;

                Context.Fields[Context.UsedNames.Count] = new(Info.FieldType, Name);

                if (!Context.UsedNames.Add(Name))
                {
                    throw new ADFRepeatedNameException(Name);
                }

                yield return GetFieldWriter(Info, Context);
            }
        }

        private static IEnumerable<(FieldInfo, string)> FilterFields(FieldInfo[] Fields)
        {
            for (int i = 0; i < Fields.Length; i++)
            {
                var Info = Fields[i];
                var Name = Info.Name;
                var IsNotAutoField = !IsAutoField(ref Name);

                if (Info.FieldType.IsAssignableTo(typeof(Delegate)))
                {
                    continue;
                }

                foreach (var Attribute in Info.GetCustomAttributes())
                {
                    if (Attribute is ADFIgnoreAttribute or NonSerializedAttribute)
                    {
                        goto Continue;
                    }

                    if (IsNotAutoField && Attribute is CompilerGeneratedAttribute)
                    {
                        goto Continue;
                    }

                    if (Attribute is ADFNameAttribute NameAttribute)
                    {
                        Name = NameAttribute.Name;
                    }
                }

                yield return (Info, Name);

                Continue:;
            }
        }


        private static Expression GetFieldWriter(FieldInfo Info, AutoSerializerBuilderContext<T> Context)
        {
            return Expression.Call
            (
                GetWriteMethod(Info.FieldType),
                Context.ContextParameter,
                Context.TargetParameter,
                Expression.Field(Context.ValueParameter, Info)
            );
        }

        private static MethodInfo GetWriteMethod(Type Type)
        {
            return WriteMethodCache.GetOrAdd(Type, GetNewWriteMethod);
        }

        private static MethodInfo GetNewWriteMethod(Type Type)
        {
            return AutoSerializerHelper.GetWriter(Type);
        }


        private static bool IsAutoField(ref string Name)
        {
            if (Name[0] == '<' && Name.EndsWith(">k__BackingField"))
            {
                Name = Name[1..^16];
                return true;
            }
            return false;
        }

        private static void CheckContext(ADFWritingContext Context, Type Type)
        {
            ArgumentNullException.ThrowIfNull(Context);
            ArgumentNullException.ThrowIfNull(Type);
            if (!Type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException($"{Type} can not assignable to {typeof(T)}");
            }
        }
    }
}