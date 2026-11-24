using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Zion.Serialization;

namespace Zion.Geometry
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2Int : IBinarySerializable<Vector2Int>, IEquatable<Vector2Int>, IEqualityComparer<Vector2Int>, IRandomizable<Vector2Int>
    {
        #region Constants
        public static readonly Vector2Int Zero   = new(0, 0);
        public static readonly Vector2Int Left   = new(-1, 0);
        public static readonly Vector2Int Right  = new(1, 0);
        public static readonly Vector2Int Down   = new(0, -1);
        public static readonly Vector2Int Up     = new(0, 1);
        public static readonly Vector2Int OneOne = new(1);
        #endregion

        #region Data
        public int X, Y;
        #endregion

        #region Properties
        public float Magnitude => MathF.Sqrt(LengthSquared);

        public int LengthSquared => X * X + Y * Y;

        public Vector2Int Normal
        {
            get
            {
                if (this == Zero)
                {
                    return Zero;
                }
                var Magnitude = this.Magnitude;
                return new Vector2Int((int)(X / Magnitude), (int)(Y / Magnitude));
            }
        }
        #endregion

        #region Constructors
        public Vector2Int(int Axes)
        {
            X = Axes;
            Y = Axes;
        }

        public Vector2Int(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        private Vector2Int(SerializationInfo Info, StreamingContext Context)
        {
            X = Info.GetInt32("X");
            Y = Info.GetInt32("Y");
        }
        #endregion

        #region Operators
        #region MathOperators
        public static Vector2Int operator +(Vector2Int A, Vector2Int B)
        {
            return new Vector2Int(A.X + B.X, A.Y + B.Y);
        }

        public static Vector2Int operator +(Vector2Int A, Direction B)
        {
            return A + B.ToVector2Int();
        }

        public static Vector2Int operator +(Vector2Int A, HorizontalDirection B)
        {
            return A with { X = A.X + (int)B };
        }

        public static Vector2Int operator +(Vector2Int A, VerticalDirection B)
        {
            return A with { Y = A.Y + (int)B };
        }

        public static Vector2Int operator +(Direction A, Vector2Int B)
        {
            return B + A;
        }

        public static Vector2Int operator +(HorizontalDirection A, Vector2Int B)
        {
            return B + A;
        }

        public static Vector2Int operator +(VerticalDirection A, Vector2Int B)
        {
            return B + A;
        }

        public static Vector2Int operator -(Vector2Int A, Vector2Int B)
        {
            return new Vector2Int(A.X - B.X, A.Y - B.Y);
        }

        public static Vector2Int operator -(Vector2Int A, Direction B)
        {
            return A - B.ToVector2Int();
        }

        public static Vector2Int operator -(Vector2Int A, HorizontalDirection B)
        {
            return A with { X = A.X - (int)B };
        }

        public static Vector2Int operator -(Vector2Int A, VerticalDirection B)
        {
            return A with { Y = A.Y - (int)B };
        }

        public static Vector2Int operator -(Direction A, Vector2Int B)
        {
            return A.ToVector2Int() - B;
        }

        public static Vector2Int operator -(HorizontalDirection A, Vector2Int B)
        {
            return B with { X = (int)A - B.X };
        }

        public static Vector2Int operator -(VerticalDirection A, Vector2Int B)
        {
            return B with { Y = (int)A - B.Y };
        }

        public static Vector2Int operator *(Vector2Int A, int B)
        {
            return new Vector2Int(A.X * B, A.Y * B);
        }

        public static Vector2Int operator /(Vector2Int A, int B)
        {
            return new Vector2Int(A.X / B, A.Y / B);
        }

        public static Vector2Int operator &(Vector2Int A, int B)
        {
            return new Vector2Int(A.X & B, A.Y & B);
        }

        public static Vector2Int operator &(Vector2Int A, Axes B)
        {
            return new Vector2Int
            (
                B.HasFlag(Axes.X) ? A.X : 0,
                B.HasFlag(Axes.Y) ? A.Y : 0
            );
        }

        public static Vector2Int operator &(Vector2Int A, Vector2Int B)
        {
            return new Vector2Int(A.X & B.X, A.Y & B.Y);
        }

        public static Vector2Int operator |(Vector2Int A, int B)
        {
            return new Vector2Int(A.X | B, A.Y | B);
        }

        public static Vector2Int operator |(Vector2Int A, Vector2Int B)
        {
            return new Vector2Int(A.X | B.X, A.Y | B.Y);
        }

        public static Vector2Int operator >> (Vector2Int A, int B)
        {
            return new Vector2Int(A.X >> B, A.Y >> B);
        }

        public static Vector2Int operator <<(Vector2Int A, int B)
        {
            return new Vector2Int(A.X << B, A.Y << B);
        }

        public static Vector2Int operator ++(Vector2Int Value)
        {
            Value.X++;
            Value.Y++;
            return Value;
        }

        public static Vector2Int operator --(Vector2Int Value)
        {
            Value.X--;
            Value.Y--;
            return Value;
        }

        public static Vector2Int operator -(Vector2Int Value)
        {
            return new Vector2Int(-Value.X, -Value.Y);
        }
        #endregion

        #region ComparisonOperators
        public static bool operator ==(Vector2Int A, Vector2Int B)
        {
            return A.X == B.X && A.Y == B.Y;
        }

        public static bool operator ==(Vector2Int A, Vector2 B)
        {
            return A.X == B.X && A.Y == B.Y;
        }

        public static bool operator !=(Vector2Int A, Vector2Int B)
        {
            return A.X != B.X || A.Y != B.Y;
        }

        public static bool operator !=(Vector2Int A, Vector2 B)
        {
            return A.X != B.X || A.Y != B.Y;
        }

        public static bool operator <(Vector2Int A, Vector2Int B)
        {
            return A.X < B.X && A.Y < B.Y;
        }

        public static bool operator <(Vector2Int A, Vector2 B)
        {
            return A.X < B.X && A.Y < B.Y;
        }

        public static bool operator >(Vector2Int A, Vector2Int B)
        {
            return A.X > B.X && A.Y > B.Y;
        }

        public static bool operator >(Vector2Int A, Vector2 B)
        {
            return A.X > B.X && A.Y > B.Y;
        }

        public static bool operator <=(Vector2Int A, Vector2Int B)
        {
            return A.X <= B.X && A.Y <= B.Y;
        }

        public static bool operator <=(Vector2Int A, Vector2 B)
        {
            return A.X <= B.X && A.Y <= B.Y;
        }

        public static bool operator >=(Vector2Int A, Vector2Int B)
        {
            return A.X >= B.X && A.Y >= B.Y;
        }

        public static bool operator >=(Vector2Int A, Vector2 B)
        {
            return A.X >= B.X && A.Y >= B.Y;
        }
        #endregion

        #region ConversionOperators
        public static explicit operator Vector2Int(Vector2 Value)
        {
            return new Vector2Int((int)Value.X, (int)Value.Y);
        }
        #endregion
        #endregion

        #region OverrideMethods
        public readonly override string ToString()
        {
            return $"[{X}; {Y}]";
        }

        public readonly override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is Vector2Int Value && this == Value;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        #endregion

        #region IEquatable & IEqualityComparer
        public bool Equals(Vector2Int Other)
        {
            return this == Other;
        }

        public bool Equals(Vector2Int A, Vector2Int B)
        {
            return A == B;
        }

        public int GetHashCode([DisallowNull] Vector2Int Object)
        {
            return Object.GetHashCode();
        }
        #endregion

        #region ISerializable & IBinarySerializable
        public void GetObjectData(SerializationInfo Info, StreamingContext Context)
        {
            Info.AddValue("X", X);
            Info.AddValue("Y", Y);
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
        }

        public static Vector2Int Read(BinaryReader Reader)
        {
            return new Vector2Int
            (
                Reader.ReadInt32(),
                Reader.ReadInt32()
            );
        }
        #endregion

        #region IRandomizable
        public static Vector2Int GetRandom(Random Random, Vector2Int Min, Vector2Int Max)
        {
            return new Vector2Int(Random.Next(Min.X, Max.X), Random.Next(Min.Y, Max.Y));
        }
        #endregion

        #region Methods
        #region Changins
        public void Normalize()
        {
            this = Normal;
        }
        #endregion

        #region Math
        public static Vector2Int Sum(params IEnumerable<Vector2Int> Values)
        {
            ArgumentNullException.ThrowIfNull(Values);

            var Result = Zero;

            foreach (var Value in Values)
            {
                Result += Value;
            }

            return Result;
        }


        public static Vector2Int Abs(in Vector2Int Value)
        {
            return new Vector2Int(Math.Abs(Value.X), Math.Abs(Value.Y));
        }

        public static Vector2Int Sign(in Vector2Int Value)
        {
            return new Vector2Int(Int32.Sign(Value.X), Int32.Sign(Value.Y));
        }


        public static Vector2Int Min(in Vector2Int A, in Vector2Int B)
        {
            return new Vector2Int(Math.Min(A.X, B.X), Math.Min(A.Y, B.Y));
        }

        public static Vector2Int Max(in Vector2Int A, in Vector2Int B)
        {
            return new Vector2Int(Math.Max(A.X, B.X), Math.Max(A.Y, B.Y));
        }

        public static Vector2Int Clamp(in Vector2Int Value, in Vector2Int Min, in Vector2Int Max)
        {
            return new Vector2Int
            (
                Math.Clamp(Value.X, Min.X, Max.X),
                Math.Clamp(Value.Y, Min.Y, Max.Y)
            );
        }


        public static int Dot(Vector2Int A, Vector2Int B)
        {
            return A.X * B.X + A.Y * B.Y;
        }


        public static float Distance(in Vector2Int A, in Vector2Int B)
        {
            return (A - B).Magnitude;
        }

        public static bool WithinRadius(in Vector2Int A, in Vector2Int B, float Radius)
        {
            return (A - B).LengthSquared <= Radius * Radius;
        }


        public static bool IsPositive(in Vector2Int Value)
        {
            return Int32.IsPositive(Value.X) && Int32.IsPositive(Value.Y);
        }

        public static bool IsNegative(in Vector2Int Value)
        {
            return Int32.IsNegative(Value.X) || Int32.IsNegative(Value.Y);
        }
        #endregion
        #endregion
    }
}