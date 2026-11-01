namespace Zion.MathExpressions
{
    public interface IMathTerm
    {
        Fraction GetValue()
        {
            return GetValue(10);
        }

        Fraction GetValue(int Accuracy);

        virtual IMathTerm Simplify()
        {
            return this;
        }
    }
}