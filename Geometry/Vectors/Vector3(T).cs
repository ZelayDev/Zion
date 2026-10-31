using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Zion.Serialization;

namespace Zion.Geometry
{
    public struct Vector3<T> : IEquatable<Vector3<T>>, IEqualityComparer<Vector3<T>>
        where T : unmanaged, INumber<T>
    {
        #region Data
        public T X, Y, Z;

        #endregion

        #region Constructors
        public Vector3(T Axes)
        {
            X = Axes;
            Y = Axes;
            Z = Axes;
        }

        public Vector3(T X, T Y, T Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        #endregion

        #region Operators
        #region MathOperators
        public static Vector3<T> operator +(Vector3<T> A, Vector3<T> B)
        {
            return new Vector3<T>(A.X + B.X, A.Y + B.Y, A.Z + B.Z);
        }

        public static Vector3<T> operator -(Vector3<T> A, Vector3<T> B)
        {
            return new Vector3<T>(A.X - B.X, A.Y - B.Y, A.Z - B.Z);
        }

        public static Vector3<T> operator *(Vector3<T> A, T B)
        {
            return new Vector3<T>(A.X * B, A.Y * B, A.Z * B);
        }

        public static Vector3<T> operator /(Vector3<T> A, T B)
        {
            return new Vector3<T>(A.X / B, A.Y / B, A.Z / B);
        }

        public static Vector3<T> operator &(Vector3<T> A, Axes B)
        {
            return new Vector3<T>
            (
                B.HasFlag(Axes.X) ? A.X : T.Zero,
                B.HasFlag(Axes.Y) ? A.Y : T.Zero,
                B.HasFlag(Axes.Z) ? A.Z : T.Zero
            );
        }

        public static Vector3<T> operator ++(Vector3<T> Value)
        {
            Value.X++;
            Value.Y++;
            Value.Z++;
            return Value;
        }

        public static Vector3<T> operator --(Vector3<T> Value)
        {
            Value.X--;
            Value.Y--;
            Value.Z--;
            return Value;
        }

        public static Vector3<T> operator -(Vector3<T> Value)
        {
            return new Vector3<T>(-Value.X, -Value.Y, -Value.Z);
        }

        #endregion

        #region ComprasionOperators
        public static bool operator ==(Vector3<T> A, Vector3<T> B)
        {
            return A.X == B.X && A.Y == B.Y && A.Z == B.Z;
        }

        public static bool operator !=(Vector3<T> A, Vector3<T> B)
        {
            return A.X != B.X || A.Y != B.Y || A.Z != B.Z;
        }

        public static bool operator <(Vector3<T> A, Vector3<T> B)
        {
            return A.X < B.X && A.Y < B.Y && A.Z < B.Z;
        }

        public static bool operator >(Vector3<T> A, Vector3<T> B)
        {
            return A.X > B.X && A.Y > B.Y && A.Z > B.Z;
        }

        public static bool operator <=(Vector3<T> A, Vector3<T> B)
        {
            return A.X <= B.X && A.Y <= B.Y && A.Z <= B.Z;
        }

        public static bool operator >=(Vector3<T> A, Vector3<T> B)
        {
            return A.X >= B.X && A.Y >= B.Y && A.Z >= B.Z;
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
            return Object is Vector3<T> Value && this == Value;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        #endregion

        #region IEquatable & IEqualityComparer
        public bool Equals(Vector3<T> Other)
        {
            return this == Other;
        }

        public bool Equals(Vector3<T> A, Vector3<T> B)
        {
            return A == B;
        }

        public int GetHashCode([DisallowNull] Vector3<T> Object)
        {
            return Object.GetHashCode();
        }

        #endregion

        #region PublicMethods
        public static int GetByteCount()
        {
            return Unsafe.SizeOf<Vector3<T>>();
        }
        #endregion
    }
}