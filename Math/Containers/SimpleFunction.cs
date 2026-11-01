namespace Zion.MathExpressions
{
    public sealed class SimpleFunction : IMathTerm
    {
        public readonly IMathTerm                      Value;
        public readonly Func<IMathTerm, int, Fraction> Function;

        public SimpleFunction(IMathTerm Value, Func<IMathTerm, int, Fraction> Function)
        {
            this.Value = Value;
            this.Function = Function;
        }

        public Fraction GetValue(int Accuracy)
        {
            return Function(Value, Accuracy);
        }
    }
}