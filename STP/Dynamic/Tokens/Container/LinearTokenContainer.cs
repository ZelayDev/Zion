using Group = System.Collections.Generic.List<Zion.STP.Token>;

namespace Zion.STP.Dynamic
{
    //Do: LinearTokenContainer realization
    public sealed class LinearTokenContainer : ITokenContainer<IntPointer>
    {
        private readonly Dictionary<IntPointer, Group> Groups;
        private readonly Group CurrentGroup;

        public int Count { get; private set; }

        public int GroupLength
        {
            private get;
            init => field = Math.Max(value, 4);
        } = 12;


        public LinearTokenContainer(int GroupCapacity = 16)
        {
            Groups = new(Math.Max(GroupCapacity, 8));
        }


        private Group NewGroup()
        {
            return new Group(GroupLength);
        }


        

        public void Add(Token Token)
        {

        }

        public void Overwrite(IntPointer Start, int RemovedTokens, IEnumerable<Token> Tokens)
        {

        }

        public IntPointer GetTokenStart(IntPointer Position)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Token> EnumeratorFrom(IntPointer Start)
        {
            throw new NotImplementedException();
        }
    }
}