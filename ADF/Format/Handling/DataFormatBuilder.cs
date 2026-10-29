namespace Zion.ADF
{
    internal static class DataFormatBuilder
    {
        public static DataFormat BuildDeferred(Type Type, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            return new DataFormat
            (
                [],
                GetGenerics(Type, FormatRegistry, TypeAssociation),
                GetFlags(Type) | FormatFlags.IsDeferred,
                GetBaseFormatId(Type, FormatRegistry, TypeAssociation)
            );
        }

        public static DataFormat BuildDeferred(Type Type, uint BaseFormatId, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            return new DataFormat
            (
                [],
                GetGenerics(Type, FormatRegistry, TypeAssociation),
                GetFlags(Type) | FormatFlags.IsDeferred,
                BaseFormatId
            );
        }

        public static DataFormat Build(Parameter[] Parameters, Type Type, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            return new DataFormat
            (
                Parameters,
                GetGenerics(Type, FormatRegistry, TypeAssociation),
                GetFlags(Type),
                GetBaseFormatId(Type, FormatRegistry, TypeAssociation)
            );
        }

        public static DataFormat Build(TypeSchema Schema, ADFWritingContext Context)
        {
            return Build
            (
                Schema,
                GetBaseFormatId(Schema.Type, Context.Registries.FormatRegistry, Context.Registries.TypeAssociation),
                Context
            );
        }

        public static DataFormat Build(TypeSchema Schema, uint BaseFormatId, ADFWritingContext Context)
        {
            var Type = Schema.Type;
            var Registries = Context.Registries;
            var FormatRegistry = Registries.FormatRegistry;
            var StringRegistry = Registries.StringRegistry;
            var TypeAssociation = Registries.TypeAssociation;

            return new DataFormat
            (
                GetParameters(Schema.Fields, FormatRegistry, TypeAssociation, StringRegistry),
                GetGenerics(Schema.Generics, FormatRegistry, TypeAssociation),
                Schema.Flags,
                BaseFormatId
            );
        }


        public static FormatFlags GetFlags(Type Type)
        {
            var Flags = FormatFlags.None;

            if (!Type.IsValueType)
            {
                Flags |= FormatFlags.IsReference;
            }
            if (Type.IsAbstract)
            {
                Flags |= FormatFlags.IsAbstract;
            }
            if (Type.IsNullable)
            {
                Flags |= FormatFlags.IsNullable;
            }

            return Flags;
        }


        private static Parameter[] GetParameters(Field[] Fields, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation, StringIdRegistry StringRegistry)
        {
            Parameter Convert(Field Field)
            {
                return new Parameter
                (
                    StringRegistry.GetOrAdd(Field.Name),
                    TypeAssociation.GetOrAddDeferred(Field.Type, FormatRegistry)
                );
            }

            return Array.ConvertAll(Fields, Convert);
        }

        private static uint[] GetGenerics(Type Type, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            return Type.IsGenericType
                ? GetGenerics(Type.GetGenericArguments(), FormatRegistry, TypeAssociation)
                : [];
        }

        private static uint[] GetGenerics(Type[] Generics, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            return Array.ConvertAll
            (
                Generics, Generic => TypeAssociation.GetOrAddDeferred(Generic, FormatRegistry)
            );
        }

        private static uint GetBaseFormatId(Type Type, FormatRegistry FormatRegistry, TypeAssociation TypeAssociation)
        {
            var Base = Type.BaseType;
            return DataFormat.HasBase(Type)
                ? TypeAssociation.GetOrAddDeferred(Base!, FormatRegistry)
                : ADFPrimitives.Object;
        }
    }
}