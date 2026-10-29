namespace Zion
{
    public static class TypeExtensions
    {
        extension(Type Type)
        {
            public bool IsNullable => !Type.IsValueType || Nullable.GetUnderlyingType(Type) is not null;
        }
    }
}