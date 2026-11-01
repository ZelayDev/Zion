using System.Numerics;

namespace Zion.STP.Dynamic
{
    public sealed partial class RelativeTokenContainer<TPointer> : ITokenContainer<TPointer>
        where TPointer : TextPointer<TPointer>
    {
        #region Data
        private BaseNode  Main;
        private FinalNode Last;

        private int GroupCount;
        #endregion

        #region Properties
        public int Count { get; private set; }

        public int TokenGroupLength
        {
            get;
            init => field = Math.Max(4, value);
        }
        #endregion

        #region IContainer
        public void Add(Token Token, TPointer Position)
        {
            Count++;

            if (GroupCount == 0)
            {
                Last = new FinalNode(Position, Token);
                Main = Last;
                GroupCount++;
                return;
            }

            if (Last.Count < TokenGroupLength)
            {
                Last.Add(Token);
                return;
            }

            if (BitOperations.IsPow2(GroupCount++))
            {
                Main.Position = Main.Position.Subtract(Position);

                Last = new FinalNode(Position, Token);
                Last.Position = Last.Position.Subtract(Position);

                Main = new Node(Position, Main, Last);
            }
            else
            {
                var New = new FinalNode(Position, Token);
                var Parent = FindRightmostEmptySlot((Node)Main);

                if (Parent.Right is null)
                {
                    New.Position = Position.Subtract(Parent.Position);
                    Parent.Right = New;
                }
                else
                {
                    var NewParentPosition = Parent.Position;
                    New.Position = Position.Subtract(NewParentPosition);

                    var NewParent = new Node(NewParentPosition, Parent.Right, New);
                    Parent.Right = NewParent;
                }

                Last = New;
            }
        }

        public void Clear()
        {
            Main = null;
            Last = null;
            Count = 0;
            GroupCount = 0;
        }

        public void Overwrite(TPointer Start, int RemovedTokens, IEnumerable<Token> Tokens)
        {
            //TODO
        }

        public TPointer GetTokenStart(TPointer Position)
        {
            return default!;
            //TODO
        }

        public IEnumerable<Token> EnumeratorFrom(TPointer Start, out TPointer FirstTokenStart)
        {
            ArgumentException.ThrowIf(!Start.IsValid, $"Start(={Start.ToNotNullString()}) is invalid");
            var Group = GetTokenGroup(Start);

            if (Group is not null)
            {
                var TokenIndex = Group.GetTokenIndex(Start, out FirstTokenStart);

                ArgumentOutOfRangeException.ThrowIf(TokenIndex == -1,
                    $"Start(={Start.ToNotNullString()}) out of range");

                return Group.Range(TokenIndex);
            }

            throw new ArgumentOutOfRangeException($"Start(={Start.ToNotNullString()}) out of range");
        }
        #endregion

        #region PrivateMethods
        private Node FindRightmostEmptySlot(Node Node)
        {
            while (Node.Right is Node RightNode)
            {
                Node = RightNode;
            }
            return Node;
        }

        private FinalNode? GetTokenGroup(TPointer Position)
        {
            var Current = Main;

            while (Current is Node Branch)
            {
                var RightStart = Branch.Right is not null
                    ? Branch.Position.Sum(Branch.Right.Position)
                    : Branch.Position;

                if (Position.CompareTo(RightStart) < 0)
                {
                    Current = Branch.Left;
                }
                else
                {
                    Current = Branch.Right;
                }
            }

            return Current as FinalNode;
        }
        #endregion
    }
}