using System.Linq.Expressions;

namespace Zion.ADF
{
    internal readonly struct AutoSerializerBuilderContext<T>
    {
        public readonly ParameterExpression ContextParameter = Expression.Parameter(typeof(ADFWritingContext), "Context");
        public readonly ParameterExpression TargetParameter = Expression.Parameter(typeof(StreamGroup), "Target");
        public readonly ParameterExpression ValueParameter = Expression.Parameter(typeof(T), "Value");

        public readonly HashSet<string> UsedNames;
        public readonly Field[] Fields;


        public AutoSerializerBuilderContext(int Capacity)
        {
            UsedNames = new(Capacity);
            Fields = new Field[Capacity];
        }


        public Field[] GetFields()
        {
            int Used = UsedNames.Count;
            return Fields.Length == Used
                ? Fields
                : Fields[..Used];
        }
    }
}