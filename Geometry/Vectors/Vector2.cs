using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Zion.Serialization;

namespace Zion.Geometry
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Vector2 : IBinarySerializable<Vector2>, IEquatable<Vector2>, IEqualityComparer<Vector2>, ISerializable, IRandomizable<Vector2>
    {
        #region Constans
        public static readonly Vector2 Zero   = new Vector2(0f, 0f);
        public static readonly Vector2 Left   = new Vector2(-1f, 0f);
        public static readonly Vector2 Right  = new Vector2(1f, 0f);
        public static readonly Vector2 Down   = new Vector2(0f, -1f);
        public static readonly Vector2 Up     = new Vector2(0f, 1f);
        public static readonly Vector2 OneOne = new Vector2(1f);

        #endregion

        #region Data
        public float X, Y;

        #endregion

        #region Properties
        public float Magnitude => MathF.Sqrt(LengthSquared);

        public float LengthSquared => (X * X) + (Y * Y);

        public Vector2 Reversed => new Vector2(Y, X);

        public Vector2 Normal
        {
            get
            {
                if (this == Zero) { return Zero; }
                float Magnitude = this.Magnitude;
                return new Vector2(X / Magnitude, Y / Magnitude);
            }
        }

        #endregion

        #region Constructors
        public Vector2(float Axes)
        {
            X = Axes;
            Y = Axes;
        }

        public Vector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Vector2(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        private Vector2(SerializationInfo Info, StreamingContext Context)
        {
            X = Info.GetSingle("X");
            Y = Info.GetSingle("Y");
        }


        #endregion

        #region Operators
        #region MathOperators
        public static Vector2 operator +(Vector2 A, Vector2 B)
        {
            return new Vector2(A.X + B.X, A.Y + B.Y);
        }
        public static Vector2 operator +(Vector2 A, Direction B)
        {
            return A + B.ToVector2();
        }
        public static Vector2 operator +(Vector2 A, HorizontalDirection B)
        {
            return A with { X = A.X + (int)B };
        }
        public static Vector2 operator +(Vector2 A, VerticalDirection B)
        {
            return A with { Y = A.Y + (int)B };
        }
        public static Vector2 operator +(Direction A, Vector2 B)
        {
            return B + A;
        }
        public static Vector2 operator +(HorizontalDirection A, Vector2 B)
        {
            return B + A;
        }
        public static Vector2 operator +(VerticalDirection A,Vector2  B)
        {
            return B + A;
        }

        public static Vector2 operator -(Vector2 A, Vector2 B)
        {
            return new Vector2(A.X - B.X, A.Y - B.Y);
        }
        public static Vector2 operator -(Vector2 A, Direction B)
        {
            return A - B.ToVector2();
        }
        public static Vector2 operator -(Vector2 A, HorizontalDirection B)
        {
            return A with { X = A.X - (int)B };
        }
        public static Vector2 operator -(Vector2 A, VerticalDirection B)
        {
            return A with { Y = A.Y - (int)B };
        }
        public static Vector2 operator -(Direction A, Vector2 B)
        {
            return A.ToVector2() - B;
        }
        public static Vector2 operator -(HorizontalDirection A, Vector2 B)
        {
            return B with { X = (int)A - B.X };
        }
        public static Vector2 operator -(VerticalDirection A, Vector2 B)
        {
            return B with { Y = (int)A - B.Y };
        }

        public static Vector2 operator *(Vector2 A, float B)
        {
            return new Vector2(A.X * B, A.Y * B);
        }
        public static Vector2 operator /(Vector2 A, float B)
        {
            return new Vector2(A.X / B, A.Y / B);
        }

        public static Vector2 operator &(Vector2 A, Axes B)
        {
            return new Vector2
            (
                B.HasFlag(Axes.X) ? A.X : 0f,
                B.HasFlag(Axes.Y) ? A.Y : 0f
            );
        }

        public static Vector2 operator ++(Vector2 Value)
        {
            Value.X++;
            Value.Y++;
            return Value;
        }
        public static Vector2 operator --(Vector2 Value)
        {
            Value.X--;
            Value.Y--;
            return Value;
        }

        public static Vector2 operator -(Vector2 Value)
        {
            return new Vector2(-Value.X, -Value.Y);
        }

        #endregion

        #region ComparisonOperators
        public static bool operator ==(Vector2 A, Vector2 B)
        {
            return A.X == B.X && A.Y == B.Y;
        }
        public static bool operator ==(Vector2 A, Vector2Int B)
        {
            return A.X == B.X && A.Y == B.Y;
        }

        public static bool operator !=(Vector2 A, Vector2 B)
        {
            return A.X != B.X || A.Y != B.Y;
        }
        public static bool operator !=(Vector2 A, Vector2Int B)
        {
            return A.X != B.X || A.Y != B.Y;
        }

        public static bool operator <(Vector2 A, Vector2 B)
        {
            return A.X < B.X && A.Y < B.Y;
        }
        public static bool operator <(Vector2 A, Vector2Int B)
        {
            return A.X < B.X && A.Y < B.Y;
        }
        
        public static bool operator >(Vector2 A, Vector2 B)
        {
            return A.X > B.X && A.Y > B.Y;
        }
        public static bool operator >(Vector2 A, Vector2Int B)
        {
            return A.X > B.X && A.Y > B.Y;
        }

        public static bool operator <=(Vector2 A, Vector2 B)
        {
            return A.X <= B.X && A.Y <= B.Y;
        }
        public static bool operator <=(Vector2 A, Vector2Int B)
        {
            return A.X <= B.X && A.Y <= B.Y;
        }

        public static bool operator >=(Vector2 A, Vector2 B)
        {
            return A.X >= B.X && A.Y >= B.Y;
        }
        public static bool operator >=(Vector2 A, Vector2Int B)
        {
            return A.X >= B.X && A.Y >= B.Y;
        }

        #endregion

        #region ConversionOperators
        public static implicit operator Vector2(Vector2Int Value)
        {
            return new Vector2(Value.X, Value.Y);
        }

        #endregion

        #endregion

        #region OverrideMethods
        public override readonly string ToString()
        {
            return $"[{X}; {Y}]";
        }

        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is Vector2 Value && this == Value;
        }
        
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        #endregion

        #region IEquatable & IEqualityComparer
        public bool Equals(Vector2 Other)
        {
            return this == Other;
        }

        public bool Equals(Vector2 A, Vector2 B)
        {
            return A == B;
        }

        public int GetHashCode([DisallowNull] Vector2 Object)
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

        public static Vector2 Read(BinaryReader Reader)
        {
            return new Vector2
            (
                Reader.ReadSingle(),
                Reader.ReadSingle()
            );
        }

        #endregion

        #region IRandomizable
        public static Vector2 GetRandom(Random Random, Vector2 Min, Vector2 Max)
        {
            return new Vector2(Random.NextFloat(Min.X, Max.X), Random.NextFloat(Min.Y, Max.Y));
        }

        #endregion

        #region Methods
        #region Conversion
        public void Reverse()
        {
            this = Reversed;
        }
        
        public void Normalize()
        {
            this = Normal;
        }

        #endregion

        #region Math
        public static Vector2 Sum(params IEnumerable<Vector2> Values)
        {
            ArgumentNullException.ThrowIfNull(Values);

            Vector2 Result = Zero;

            foreach (Vector2 Value in Values)
            {
                Result += Value;
            }

            return Result;
        }


        public static Vector2 Abs(in Vector2 Value)
        {
            return new Vector2(Math.Abs(Value.X), Math.Abs(Value.Y));
        }

        public static Vector2 Sign(in Vector2 Value)
        {
            return new Vector2(float.Sign(Value.X), float.Sign(Value.Y));
        }


        public static Vector2 Min(in Vector2 A, in Vector2 B)
        {
            return new Vector2(Math.Min(A.X, B.X), Math.Min(A.Y, B.Y));
        }

        public static Vector2 Max(in Vector2 A, in Vector2 B)
        {
            return new Vector2(Math.Max(A.X, B.X), Math.Max(A.Y, B.Y));
        }

        public static Vector2 Clamp(in Vector2 Value, in Vector2 Min, in Vector2 Max)
        {
            return new Vector2
            (
                Math.Clamp(Value.X, Min.X, Max.X),
                Math.Clamp(Value.Y, Min.Y, Max.Y)
            );
        }


        public static Vector2 Lerp(in Vector2 A, in Vector2 B, in float Alpha)
        {
            return A + ((B - A) * Alpha);
        }

        public static Vector2 MoveTowards(in Vector2 Current, in Vector2 Target, in float MaxDelta)
        {
            Vector2 Difference = Target - Current;
            float DistanceSquared = Difference.LengthSquared;

            return DistanceSquared == 0f || DistanceSquared <= MaxDelta * MaxDelta
                ? Target
                : Current + Difference / MathF.Sqrt(DistanceSquared) * MaxDelta;
        }

        public static Vector2 Project(in Vector2 A, in Vector2 B)
        {
            return B == Zero ? Zero : B * (Dot(A, B) / B.LengthSquared);
        }

        public static Vector2 Reflect(in Vector2 Value, in Vector2 Normal)
        {
            return Value - Normal * 2f * Dot(Value, Normal);
        }


        public static float Dot(in Vector2 A, in Vector2 B)
        {
            return A.X * B.X + A.Y * B.Y;
        }


        public static float Distance(in Vector2 A, in Vector2 B)
        {
            return (A - B).Magnitude;
        }

        public static bool WithinRadius(in Vector2 A, in Vector2 B, in float Radius)
        {
            return (A - B).LengthSquared <= Radius * Radius;
        }


        public static bool IsPositive(in Vector2 Value)
        {
            return float.IsPositive(Value.X) && float.IsPositive(Value.Y);
        }
        
        public static bool IsNegative(in Vector2 Value)
        {
            return float.IsNegative(Value.X) || float.IsNegative(Value.Y);
        }

        #endregion

        #endregion
    }
}