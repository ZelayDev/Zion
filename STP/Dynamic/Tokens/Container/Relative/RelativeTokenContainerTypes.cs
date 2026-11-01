using System.Collections;

namespace Zion.STP.Dynamic
{
    public sealed partial class RelativeTokenContainer<TPointer> : ITokenContainer<TPointer>
        where TPointer : TextPointer<TPointer>
    {
        private abstract class BaseNode : IEnumerable<Token>
        {
            public TPointer Position;

            public BaseNode(TPointer Position)
            {
                this.Position = Position;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public abstract IEnumerator<Token> GetEnumerator();
        }

        private sealed class Node : BaseNode
        {
            public readonly BaseNode? Left;
            public          BaseNode? Right;

            public Node(TPointer Position, BaseNode? Left) : base(Position)
            {
                this.Left = Left;
            }

            public Node(TPointer Position, BaseNode? Left, BaseNode? Right) : this(Position, Left)
            {
                this.Right = Right;
            }

            public override IEnumerator<Token> GetEnumerator()
            {
                if (Left is not null)
                {
                    foreach (var Token in Left)
                    {
                        yield return Token;
                    }
                }
                if (Right is not null)
                {
                    foreach (var Token in Right)
                    {
                        yield return Token;
                    }
                }
            }
        }

        private sealed class FinalNode : BaseNode
        {
            private readonly List<Token> Tokens;

            public int Count => Tokens.Count;

            public FinalNode(TPointer Position) : base(Position)
            {
                Tokens = new List<Token>();
            }

            public FinalNode(TPointer Position, Token Token) : this(Position)
            {
                Add(Token);
            }


            public void Add(Token Token)
            {
                Tokens.Add(Token);
            }

            public int GetTokenIndex(TPointer Target, out TPointer Start)
            {
                var LocalTargetPosition = Target.Subtract(Position);
                var TokenStart = LocalTargetPosition;

                var Count = Tokens.Count;

                for (var i = 0; i < Tokens.Count; i++)
                {
                    if (TokenStart.CompareTo(LocalTargetPosition) >= 0)
                    {
                        Start = TokenStart;
                        return i;
                    }

                    TokenStart = TokenStart.Sum(GetSize(Tokens[i]));
                }

                Start = default!;
                return -1;
            }

            public TPointer GetSize(Token Token)
            {
                throw new NotImplementedException();
                //TODO
            }


            public IEnumerable<Token> Range(int Start)
            {
                return Tokens.Range(Start);
            }


            public override IEnumerator<Token> GetEnumerator()
            {
                return Tokens.GetEnumerator();
            }
        }
    }
}