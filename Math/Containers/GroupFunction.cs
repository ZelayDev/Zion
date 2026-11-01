namespace Zion.MathExpressions
{
    public sealed class GroupFunction : IMathTerm
    {
        private readonly List<IMathTerm>                      Members;
        private readonly Func<List<IMathTerm>, int, Fraction> Function;

        public GroupFunction(List<IMathTerm> Members, Func<List<IMathTerm>, int, Fraction> Function)
        {
            this.Members = Members;
            this.Function = Function;
        }

        public Fraction GetValue(int Accuracy)
        {
            return Function(Members, Accuracy);
        }
    }
}