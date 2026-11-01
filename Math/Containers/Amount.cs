namespace Zion.MathExpressions
{
    public sealed class Amount : IMathTerm
    {
        private readonly List<IMathTerm> Members;

        public Amount()
        {
            Members = new List<IMathTerm>(3);
        }

        public Amount(params List<IMathTerm> Members)
        {
            this.Members = Members;
        }

        public Fraction GetValue(int Accuracy)
        {
            return Fraction.Sum(List.Convert(Members, Value => Value.GetValue(Accuracy)));
        }

        public IMathTerm Simplify()
        {
            return Members.Count == 1 ? Members[0] : this;
        }

        public T Add<T>(T Member) where T : IMathTerm
        {
            Members.Add(Member);
            return Member;
        }
    }
}