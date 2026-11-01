namespace Zion.MathExpressions
{
    public readonly struct Negative : IMathTerm
    {
        public readonly IMathTerm Value;

        public Negative(IMathTerm Value)
        {
            this.Value = Value;
        }

        public Fraction GetValue(int Accuracy)
        {
            return -Value.GetValue(Accuracy);
        }
    }
}