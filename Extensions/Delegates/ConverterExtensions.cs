namespace Zion
{
    public static class ConverterExtensions
    {
        public static SafeConverter<string, T> TryParse<T>(this Func<string, T?> Parser)
        {
            return (String, out Value) =>
            {
                var Input = Parser(String);
                Value = Input ?? default!;
                return Input is not null;
            };
        }

        public static Func<TIn, bool> ToFunc<TIn, TOut>(this SafeConverter<TIn, TOut> Converter)
        {
            return In => Converter(In, out _);
        }
    }
}