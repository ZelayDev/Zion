using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Zion.Serialization;

namespace Zion.Geometry
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Vector3Int : IBinarySerializable<Vector3Int>, IEquatable<Vector3Int>, IEqualityComparer<Vector3Int>, ISerializable, IRandomizable<Vector3Int>
    {
        #region Constants
        public static readonly Vector3Int Zero = new Vector3Int(0);
        public static readonly Vector3Int Left = new Vector3Int(-1, 0, 0);
        public static readonly Vector3Int Right = new Vector3Int(1, 0, 0);
        public static readonly Vector3Int Down = new Vector3Int(0, -1, 0);
        public static readonly Vector3Int Up = new Vector3Int(0, 1, 0);
        public static readonly Vector3Int Back = new Vector3Int(0, 0, -1);
        public static readonly Vector3Int Forward = new Vector3Int(0, 0, 1);

        #endregion

        #region Data
        public int X, Y, Z;

        #endregion

        #region Properties
        public float Magnitude => MathF.Sqrt(LengthSquared);

        public int LengthSquared => (X * X) + (Y * Y) + (Z * Z);

        public Vector3Int Normal
        {
            get
            {
                if (this == Zero) { return Zero; }
                float Magnitude = this.Magnitude;
                return new Vector3Int((int)(X / Magnitude), (int)(Y / Magnitude), (int)(Z / Magnitude));
            }
        }

        #endregion

        #region Constructors
        public Vector3Int(int Axes)
        {
            X = Axes;
            Y = Axes;
            Z = Axes;
        }
        
        public Vector3Int(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        private Vector3Int(SerializationInfo Info, StreamingContext Context)
        {
            X = Info.GetInt32("X");
            Y = Info.GetInt32("Y");
            Z = Info.GetInt32("Z");
        }


        #endregion

        #region Operators
        #region MathOperators
        public static Vector3Int operator +(Vector3Int A, Vector3Int B)
        {
            return new Vector3Int(A.X + B.X, A.Y + B.Y, A.Z + B.Z);
        }
        public static Vector3Int operator +(Vector3Int A, Direction B)
        {
            return A + B.ToVector3Int();
        }
        public static Vector3Int operator +(Vector3Int A, Direction3 B)
        {
            return A + B.ToVector3Int();
        }
        public static Vector3Int operator +(Vector3Int A, HorizontalDirection B)
        {
            return A with { X = A.X + (int)B };
        }
        public static Vector3Int operator +(Vector3Int A, VerticalDirection B)
        {
            return A with { Y = A.Y + (int)B };
        }
        public static Vector3Int operator +(Vector3Int A, DepthDirection B)
        {
            return A with { Z = A.Z + (int)B };
        }
        public static Vector3Int operator +(Direction A, Vector3Int B)
        {
            return B + A;
        }
        public static Vector3Int operator +(Direction3 A, Vector3Int B)
        {
            return B + A;
        }
        public static Vector3Int operator +(HorizontalDirection A, Vector3Int B)
        {
            return B + A;
        }
        public static Vector3Int operator +(VerticalDirection A, Vector3Int B)
        {
            return B + A;
        }
        public static Vector3Int operator +(DepthDirection A, Vector3Int B)
        {
            return B + A;
        }

        public static Vector3Int operator -(Vector3Int A, Vector3Int B)
        {
            return new Vector3Int(A.X - B.X, A.Y - B.Y, A.Z - B.Z);
        }
        public static Vector3Int operator -(Vector3Int A, Direction B)
        {
            return A - B.ToVector3Int();
        }
        public static Vector3Int operator -(Vector3Int A, Direction3 B)
        {
            return A - B.ToVector3Int();
        }
        public static Vector3Int operator -(Vector3Int A, HorizontalDirection B)
        {
            return A with { X = A.X - (int)B };
        }
        public static Vector3Int operator -(Vector3Int A, VerticalDirection B)
        {
            return A with { Y = A.Y - (int)B };
        }
        public static Vector3Int operator -(Vector3Int A, DepthDirection B)
        {
            return A with { Z = A.Z - (int)B };
        }
        public static Vector3Int operator -(Direction A, Vector3Int B)
        {
            return A.ToVector3Int() - B;
        }
        public static Vector3Int operator -(Direction3 A, Vector3Int B)
        {
            return A.ToVector3Int() - B;
        }
        public static Vector3Int operator -(HorizontalDirection A, Vector3Int B)
        {
            return B with { X = (int)A - B.X };
        }
        public static Vector3Int operator -(VerticalDirection A, Vector3Int B)
        {
            return B with { Y = (int)A - B.Y };
        }
        public static Vector3Int operator -(DepthDirection A, Vector3Int B)
        {
            return B with { Z = (int)A - B.Z };
        }

        public static Vector3Int operator *(Vector3Int A, int B)
        {
            return new Vector3Int(A.X * B, A.Y * B, A.Z * B);
        }
        public static Vector3Int operator /(Vector3Int A, int B)
        {
            return new Vector3Int(A.X / B, A.Y / B, A.Z / B);
        }

        public static Vector3Int operator &(Vector3Int A, Axes B)
        {
            return new Vector3Int
            (
                B.HasFlag(Axes.X) ? A.X : 0,
                B.HasFlag(Axes.Y) ? A.Y : 0,
                B.HasFlag(Axes.Z) ? A.Z : 0
            );
        }

        public static Vector3Int operator ++(Vector3Int Value)
        {
            Value.X++;
            Value.Y++;
            Value.Z++;
            return Value;
        }
        public static Vector3Int operator --(Vector3Int Value)
        {
            Value.X--;
            Value.Y--;
            Value.Z--;
            return Value;
        }

        public static Vector3Int operator -(Vector3Int Value)
        {
            return new Vector3Int(-Value.X, -Value.Y, -Value.Z);
        }

        #endregion

        #region ComprasionOperators
        public static bool operator ==(Vector3Int A, Vector3Int B)
        {
            return A.X == B.X && A.Y == B.Y && A.Z == B.Z;
        }
        public static bool operator ==(Vector3Int A, Vector3 B)
        {
            return A.X == B.X && A.Y == B.Y && A.Z == B.Z;
        }

        public static bool operator !=(Vector3Int A, Vector3Int B)
        {
            return A.X != B.X || A.Y != B.Y || A.Z != B.Z;
        }
        public static bool operator !=(Vector3Int A, Vector3 B)
        {
            return A.X != B.X || A.Y != B.Y || A.Z != B.Z;
        }

        public static bool operator <(Vector3Int A, Vector3Int B)
        {
            return A.X < B.X && A.Y < B.Y && A.Z < B.Z;
        }
        public static bool operator <(Vector3Int A, Vector3 B)
        {
            return A.X < B.X && A.Y < B.Y && A.Z < B.Z;
        }

        public static bool operator >(Vector3Int A, Vector3Int B)
        {
            return A.X > B.X && A.Y > B.Y && A.Z > B.Z;
        }
        public static bool operator >(Vector3Int A, Vector3 B)
        {
            return A.X > B.X && A.Y > B.Y && A.Z > B.Z;
        }

        public static bool operator <=(Vector3Int A, Vector3Int B)
        {
            return A.X <= B.X && A.Y <= B.Y && A.Z <= B.Z;
        }
        public static bool operator <=(Vector3Int A, Vector3 B)
        {
            return A.X <= B.X && A.Y <= B.Y && A.Z <= B.Z;
        }

        public static bool operator >=(Vector3Int A, Vector3Int B)
        {
            return A.X >= B.X && A.Y >= B.Y && A.Z >= B.Z;
        }
        public static bool operator >=(Vector3Int A, Vector3 B)
        {
            return A.X >= B.X && A.Y >= B.Y && A.Z >= B.Z;
        }
        #endregion

        #region ConversionOperators
        public static implicit operator Vector3Int(Vector3 Value)
        {
            return new Vector3Int((int)Value.X, (int)Value.Y, (int)Value.Z);
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
            return Object is Vector3Int Value && this == Value;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        #endregion

        #region IEquatable & IEqualityComparer
        public bool Equals(Vector3Int Other)
        {
            return this == Other;
        }

        public bool Equals(Vector3Int A, Vector3Int B)
        {
            return A == B;
        }

        public int GetHashCode([DisallowNull] Vector3Int Object)
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

        public static Vector3Int Read(BinaryReader Reader)
        {
            return new Vector3Int
            (
                Reader.ReadInt32(),
                Reader.ReadInt32(),
                Reader.ReadInt32()
            );
        }

        #endregion

        #region IRandomizable
        public static Vector3Int GetRandom(Random Random, Vector3Int Min, Vector3Int Max)
        {
            return new Vector3Int
            (
                Random.Next(Min.X, Max.X),
                Random.Next(Min.Y, Max.Y),
                Random.Next(Min.Z, Max.Z)
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
        public static Vector3Int Sum(params IEnumerable<Vector3Int> Values)
        {
            ArgumentNullException.ThrowIfNull(Values);

            Vector3Int Result = Zero;

            foreach (Vector3Int Value in Values)
            {
                Result += Value;
            }

            return Result;
        }


        public static Vector3Int Abs(in Vector3Int Value)
        {
            return new Vector3Int
            (
                Math.Abs(Value.X),
                Math.Abs(Value.Y),
                Math.Abs(Value.Z)
            );
        }

        public static Vector3Int Sign(in Vector3Int Value)
        {
            return new Vector3Int(int.Sign(Value.X), int.Sign(Value.Y), int.Sign(Value.Z));
        }


        public static Vector3Int Min(in Vector3Int A, in Vector3Int B)
        {
            return new Vector3Int(Math.Min(A.X, B.X), Math.Min(A.Y, B.Y), Math.Min(A.Z, B.Z));
        }

        public static Vector3Int Max(in Vector3Int A, in Vector3Int B)
        {
            return new Vector3Int(Math.Max(A.X, B.X), Math.Max(A.Y, B.Y), Math.Max(A.Z, B.Z));
        }

        public static Vector3Int Clamp(in Vector3Int Value, in Vector3Int Min, in Vector3Int Max)
        {
            return new Vector3Int
            (
                Math.Clamp(Value.X, Min.X, Max.X),
                Math.Clamp(Value.Y, Min.Y, Max.Y),
                Math.Clamp(Value.Z, Min.Z, Max.Z)
            );
        }


        public static int Dot(Vector3Int A, Vector3Int B)
        {
            return A.X * B.X + A.Y * B.Y + A.Z * B.Z;
        }

        public static Vector3Int Cross(Vector3Int A, Vector3Int B)
        {
            return new Vector3Int
            (
                A.Y * B.Z - A.Z * B.Y,
                A.Z * B.X - A.X * B.Z,
                A.X * B.Y - A.Y * B.X
            );
        }


        public static float Distance(in Vector3Int A, in Vector3Int B)
        {
            return (A - B).Magnitude;
        }

        public static bool WithinRadius(in Vector3Int A, in Vector3Int B, float Radius)
        {
            return (A - B).LengthSquared <= Radius * Radius;
        }


        public static bool IsPositive(in Vector3Int Value)
        {
            return int.IsPositive(Value.X) && int.IsPositive(Value.Y) && int.IsPositive(Value.Z);
        }

        public static bool IsNegative(in Vector3Int Value)
        {
            return int.IsNegative(Value.X) || int.IsNegative(Value.Y) || int.IsNegative(Value.Z);
        }

        #endregion

        #endregion
    }
}