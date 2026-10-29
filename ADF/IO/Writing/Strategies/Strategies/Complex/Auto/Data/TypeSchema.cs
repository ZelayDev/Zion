namespace Zion.ADF
{
    internal readonly struct TypeSchema
    {
        public readonly Type Type;
        public readonly Type[] Generics;
        public readonly Field[] Fields;
        public readonly FormatFlags Flags;

        internal TypeSchema(Type Type, Field[] Fields)
        {
            this.Type = Type.NotNull();
            this.Fields = Fields.NotNull();

            Generics = Type.GetGenericArguments();
            Flags = DataFormatBuilder.GetFlags(Type) | FormatFlags.IsGenerated;
        }
    }
}