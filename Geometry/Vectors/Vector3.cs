using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Zion.Serialization;

namespace Zion.Geometry
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Vector3 : IBinarySerializable<Vector3>, IEquatable<Vector3>, IEqualityComparer<Vector3>,  IRandomizable<Vector3>
    {
        #region Constants
        public static readonly Vector3 Zero    = new Vector3(0f);
        public static readonly Vector3 Left    = new Vector3(-1f, 0f, 0f);
        public static readonly Vector3 Right   = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 Down    = new Vector3(0f, -1f, 0f);
        public static readonly Vector3 Up      = new Vector3(0f, 1f, 0f);
        public static readonly Vector3 Back    = new Vector3(0f, 0f, -1f);
        public static readonly Vector3 Forward = new Vector3(0f, 0f, 1f);

        #endregion

        #region Data
        public float X, Y, Z;

        #endregion

        #region Properties
        public float Magnitude => MathF.Sqrt(LengthSquared);

        public float LengthSquared => (X * X) + (Y * Y) + (Z * Z);

        public Vector3 Normal
        {
            get
            {
                if (this == Zero) { return Zero; }
                float Magnitude = this.Magnitude;
                return new Vector3(X / Magnitude, Y / Magnitude, Z / Magnitude);
            }
        }

        #endregion

        #region Constructors
        public Vector3(float Axes)
        {
            X = Axes;
            Y = Axes;
            Z = Axes;
        }
        
        public Vector3(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        private Vector3(SerializationInfo Info, StreamingContext Context)
        {
            X = Info.GetSingle("X");
            Y = Info.GetSingle("Y");
            Z = Info.GetSingle("Z");
        }


        #endregion

        #region Operators
        #region MathOperators
        public static Vector3 operator +(Vector3 A, Vector3 B)
        {
            return new Vector3(A.X + B.X, A.Y + B.Y, A.Z + B.Z);
        }
        public static Vector3 operator +(Vector3 A, Direction B)
        {
            return A + B.ToVector3();
        }
        public static Vector3 operator +(Vector3 A, Direction3 B)
        {
            return A + B.ToVector3();
        }
        public static Vector3 operator +(Vector3 A, HorizontalDirection B)
        {
            return A with { X = A.X + (int)B };
        }
        public static Vector3 operator +(Vector3 A, VerticalDirection B)
        {
            return A with { Y = A.Y + (int)B };
        }
        public static Vector3 operator +(Vector3 A, DepthDirection B)
        {
            return A with { Z = A.Z + (int)B };
        }
        public static Vector3 operator +(Direction A, Vector3 B)
        {
            return B + A;
        }
        public static Vector3 operator +(Direction3 A, Vector3 B)
        {
            return B + A;
        }
        public static Vector3 operator +(HorizontalDirection A, Vector3 B)
        {
            return B + A;
        }
        public static Vector3 operator +(VerticalDirection A, Vector3 B)
        {
            return B + A;
        }
        public static Vector3 operator +(DepthDirection A, Vector3 B)
        {
            return B + A;
        }

        public static Vector3 operator -(Vector3 A, Vector3 B)
        {
            return new Vector3(A.X - B.X, A.Y - B.Y, A.Z - B.Z);
        }
        public static Vector3 operator -(Vector3 A, Direction B)
        {
            return A - B.ToVector3();
        }
        public static Vector3 operator -(Vector3 A, Direction3 B)
        {
            return A - B.ToVector3();
        }
        public static Vector3 operator -(Vector3 A, HorizontalDirection B)
        {
            return A with { X = A.X - (int)B };
        }
        public static Vector3 operator -(Vector3 A, VerticalDirection B)
        {
            return A with { Y = A.Y - (int)B };
        }
        public static Vector3 operator -(Vector3 A, DepthDirection B)
        {
            return A with { Z = A.Z - (int)B };
        }
        public static Vector3 operator -(Direction A, Vector3 B)
        {
            return A.ToVector3() - B;
        }
        public static Vector3 operator -(Direction3 A, Vector3 B)
        {
            return A.ToVector3() - B;
        }
        public static Vector3 operator -(HorizontalDirection A, Vector3 B)
        {
            return B with { X = (int)A - B.X };
        }
        public static Vector3 operator -(VerticalDirection A, Vector3 B)
        {
            return B with { Y = (int)A - B.Y };
        }
        public static Vector3 operator -(DepthDirection A, Vector3 B)
        {
            return B with { Z = (int)A - B.Z };
        }

        public static Vector3 operator *(Vector3 A, float B)
        {
            return new Vector3(A.X * B, A.Y * B, A.Z * B);
        }
        public static Vector3 operator /(Vector3 A, float B)
        {
            return new Vector3(A.X / B, A.Y / B, A.Z / B);
        }

        public static Vector3 operator &(Vector3 A, Axes B)
        {
            return new Vector3
            (
                B.HasFlag(Axes.X) ? A.X : 0f,
                B.HasFlag(Axes.Y) ? A.Y : 0f,
                B.HasFlag(Axes.Z) ? A.Z : 0f
            );
        }

        public static Vector3 operator ++(Vector3 Value)
        {
            Value.X++;
            Value.Y++;
            Value.Z++;
            return Value;
        }
        public static Vector3 operator --(Vector3 Value)
        {
            Value.X--;
            Value.Y--;
            Value.Z--;
            return Value;
        }

        public static Vector3 operator -(Vector3 Value)
        {
            return new Vector3(-Value.X, -Value.Y, -Value.Z);
        }

        #endregion

        #region ComprasionOperators
        public static bool operator ==(Vector3 A, Vector3 B)
        {
            return A.X == B.X && A.Y == B.Y && A.Z == B.Z;
        }
        public static bool operator ==(Vector3 A, Vector3Int B)
        {
            return A.X == B.X && A.Y == B.Y && A.Z == B.Z;
        }
        
        public static bool operator !=(Vector3 A, Vector3 B)
        {
            return A.X != B.X || A.Y != B.Y || A.Z != B.Z;
        }
        public static bool operator !=(Vector3 A, Vector3Int B)
        {
            return A.X != B.X || A.Y != B.Y || A.Z != B.Z;
        }

        public static bool operator <(Vector3 A, Vector3 B)
        {
            return A.X < B.X && A.Y < B.Y && A.Z < B.Z;
        }
        public static bool operator <(Vector3 A, Vector3Int B)
        {
            return A.X < B.X && A.Y < B.Y && A.Z < B.Z;
        }

        public static bool operator >(Vector3 A, Vector3 B)
        {
            return A.X > B.X && A.Y > B.Y && A.Z > B.Z;
        }
        public static bool operator >(Vector3 A, Vector3Int B)
        {
            return A.X > B.X && A.Y > B.Y && A.Z > B.Z;
        }

        public static bool operator <=(Vector3 A, Vector3 B)
        {
            return A.X <= B.X && A.Y <= B.Y && A.Z <= B.Z;
        }
        public static bool operator <=(Vector3 A, Vector3Int B)
        {
            return A.X <= B.X && A.Y <= B.Y && A.Z <= B.Z;
        }

        public static bool operator >=(Vector3 A, Vector3 B)
        {
            return A.X >= B.X && A.Y >= B.Y && A.Z >= B.Z;
        }
        public static bool operator >=(Vector3 A, Vector3Int B)
        {
            return A.X >= B.X && A.Y >= B.Y && A.Z >= B.Z;
        }

        #endregion

        #region ConversionOperators
        public static implicit operator Vector3(Vector3Int Value)
        {
            return new Vector3(Value.X, Value.Y, Value.Z);
        }

        #endregion

        #endregion

        #region OverrideMethods
        public override readonly string ToString()
        {
            return $"[{X}; {Y}; {Z}]";
        }

        public override readonly bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is Vector3 Value && this == Value;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        #endregion

        #region IEquatable & IEqualityComparer
        public bool Equals(Vector3 Other)
        {
            return this == Other;
        }

        public bool Equals(Vector3 A, Vector3 B)
        {
            return A == B;
        }

        public int GetHashCode([DisallowNull] Vector3 Object)
        {
            return Object.GetHashCode();
        }

        #endregion

        #region ISerializable & IBinarySerializable
        public void GetObjectData(SerializationInfo Info, StreamingContext Context)
        {
            Info.AddValue("X", X);
            Info.AddValue("Y", Y);
            Info.AddValue("Z", Z);
        }

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(X);
            Writer.Write(Y);
            Writer.Write(Z);
        }

        public static Vector3 Read(BinaryReader Reader)
        {
            return new Vector3
            (
                Reader.ReadSingle(),
                Reader.ReadSingle(),
                Reader.ReadSingle()
            );
        }

        #endregion

        #region IRandomizable
        public static Vector3 GetRandom(Random Random, Vector3 Min, Vector3 Max)
        {
            return new Vector3
            (
                Random.NextFloat(Min.X, Max.X),
                Random.NextFloat(Min.Y, Max.Y),
                Random.NextFloat(Min.Z, Max.Z)
            );
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
        public static Vector3 Sum(params IEnumerable<Vector3> Values)
        {
            ArgumentNullException.ThrowIfNull(Values);

            Vector3 Result = Zero;

            foreach (Vector3 Value in Values)
            {
                Result += Value;
            }

            return Result;
        }


        public static Vector3 Abs(in Vector3 Value)
        {
            return new Vector3
            (
                Math.Abs(Value.X),
                Math.Abs(Value.Y),
                Math.Abs(Value.Z)
            );
        }

        public static Vector3 Sign(in Vector3 Value)
        {
            return new Vector3(float.Sign(Value.X), float.Sign(Value.Y), float.Sign(Value.Z));
        }


        public static Vector3 Min(in Vector3 A, in Vector3 B)
        {
            return new Vector3(Math.Min(A.X, B.X), Math.Min(A.Y, B.Y), Math.Min(A.Z, B.Z));
        }

        public static Vector3 Max(in Vector3 A, in Vector3 B)
        {
            return new Vector3(Math.Max(A.X, B.X), Math.Max(A.Y, B.Y), Math.Max(A.Z, B.Z));
        }

        public static Vector3 Clamp(in Vector3 Value, in Vector3 Min, in Vector3 Max)
        {
            return new Vector3
            (
                Math.Clamp(Value.X, Min.X, Max.X),
                Math.Clamp(Value.Y, Min.Y, Max.Y),
                Math.Clamp(Value.Z, Min.Z, Max.Z)
            );
        }

        
        public static Vector3 Lerp(in Vector3 A, in Vector3 B, float Alpha)
        {
            return A + ((B - A) * Alpha);
        }

        public static Vector3 MoveTowards(Vector3 Current, Vector3 Target, float MaxDelta)
        {
            Vector3 Difference = Target - Current;
            float DistanceSquared = Difference.LengthSquared;

            return DistanceSquared == 0f || DistanceSquared <= MaxDelta * MaxDelta
                ? Target
                : Current + Difference / MathF.Sqrt(DistanceSquared) * MaxDelta;
        }

        public static Vector3 Project(Vector3 A, Vector3 B)
        {
            return B == Zero ? Zero : B * (Dot(A, B) / B.LengthSquared);
        }

        public static Vector3 Reflect(Vector3 Value, Vector3 Normal)
        {
            return Value - Normal * 2f * Dot(Value, Normal);
        }


        public static float Dot(Vector3 A, Vector3 B)
        {
            return A.X * B.X + A.Y * B.Y + A.Z * B.Z;
        }

        public static Vector3 Cross(Vector3 A, Vector3 B)
        {
            return new Vector3
            (
                A.Y * B.Z - A.Z * B.Y,
                A.Z * B.X - A.X * B.Z,
                A.X * B.Y - A.Y * B.X
            );
        }


        public static float Distance(in Vector3 A, in Vector3 B)
        {
            return (A - B).Magnitude;
        }

        public static bool WithinRadius(in Vector3 A, in Vector3 B, float Radius)
        {
            return (A - B).LengthSquared <= Radius * Radius;
        }


        public static bool IsPositive(in Vector3 Value)
        {
            return float.IsPositive(Value.X) && float.IsPositive(Value.Y) && float.IsPositive(Value.Z);
        }

        public static bool IsNegative(in Vector3 Value)
        {
            return float.IsNegative(Value.X) || float.IsNegative(Value.Y) || float.IsNegative(Value.Z);
        }

        #endregion

        #endregion
    }
}