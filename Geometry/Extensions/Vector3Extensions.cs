using System.Numerics;

namespace Zion.Geometry
{
    public static class Vector3Extensions
    {
        #region Operators
        #region Comparison
        extension<T>(Vector3<T> Value) where T : IEquatable<T>
        {
            public static bool operator ==(Vector3<T> A, Vector3<T> B)
            {
                return A.X.Equals(B.X) && A.Y.Equals(B.Y) && A.Z.Equals(B.Z);
            }

            public static bool operator !=(Vector3<T> A, Vector3<T> B)
            {
                return !(A == B);
            }
        }

        extension<T>(Vector3<T> Value) where T : IComparisonOperators<T, T, bool>
        {
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
        }
        #endregion

        #region Math
        extension<T>(Vector3<T> Value) where T : IAdditionOperators<T, T, T>
        {
            public static Vector3<T> operator +(Vector3<T> A, Vector3<T> B)
            {
                return new Vector3<T>(A.X + B.X, A.Y + B.Y, A.Z + B.Z);
            }
        }

        extension<T>(Vector3<T> Value) where T : ISubtractionOperators<T, T, T>
        {
            public static Vector3<T> operator -(Vector3<T> A, Vector3<T> B)
            {
                return new Vector3<T>(A.X - B.X, A.Y - B.Y, A.Z - B.Z);
            }
        }

        extension<T>(Vector3<T> Value) where T : IMultiplyOperators<T, T, T>
        {
            public static Vector3<T> operator *(Vector3<T> A, T B)
            {
                return new Vector3<T>(A.X * B, A.Y * B, A.Z * B);
            }
        }

        extension<T>(Vector3<T> Value) where T : IDivisionOperators<T, T, T>
        {
            public static Vector3<T> operator /(Vector3<T> A, T B)
            {
                return new Vector3<T>(A.X / B, A.Y / B, A.Z / B);
            }
        }

        extension<T>(Vector3<T> Value) where T : IModulusOperators<T, T, T>
        {
            public static Vector3<T> operator %(Vector3<T> A, T B)
            {
                return new Vector3<T>(A.X % B, A.Y % B, A.Z % B);
            }
        }

        extension<T>(Vector3<T> Value) where T : IUnaryNegationOperators<T, T>
        {
            public static Vector3<T> operator -(Vector3<T> A)
            {
                return new Vector3<T>(-A.X, -A.Y, -A.Z);
            }
        }
        #endregion

        #region Binary
        extension<T>(Vector3<T> Value) where T : IBitwiseOperators<T, T, T>
        {
            public static Vector3<T> operator &(Vector3<T> A, T B)
            {
                return new Vector3<T>(A.X & B, A.Y & B, A.Z & B);
            }

            public static Vector3<T> operator |(Vector3<T> A, T B)
            {
                return new Vector3<T>(A.X | B, A.Y | B, A.Z | B);
            }

            public static Vector3<T> operator ^(Vector3<T> A, T B)
            {
                return new Vector3<T>(A.X ^ B, A.Y ^ B, A.Z ^ B);
            }

            public static Vector3<T> operator ~(Vector3<T> A)
            {
                return new Vector3<T>(~A.X, ~A.Y, ~A.Z);
            }
        }

        extension<T>(Vector3<T> Value) where T : IShiftOperators<T, T, T>
        {
            public static Vector3<T> operator <<(Vector3<T> A, T B)
            {
                return new Vector3<T>(A.X << B, A.Y << B, A.Z << B);
            }

            public static Vector3<T> operator >> (Vector3<T> A, T B)
            {
                return new Vector3<T>(A.X >> B, A.Y >> B, A.Z >> B);
            }

            public static Vector3<T> operator >>> (Vector3<T> A, T B)
            {
                return new Vector3<T>(A.X >>> B, A.Y >>> B, A.Z >>> B);
            }
        }
        #endregion

        #region Functions
        extension<T>(Vector3<T> Value) where T : INumberBase<T>
        {
            public static Vector3<T> Zero      => new(T.Zero);
            public static Vector3<T> Left      => new(-T.One, T.Zero, T.Zero);
            public static Vector3<T> Right     => new(T.One, T.Zero, T.Zero);
            public static Vector3<T> Down      => new(T.Zero, -T.One, T.Zero);
            public static Vector3<T> Up        => new(T.Zero, T.One, T.Zero);
            public static Vector3<T> Back      => new(T.Zero, T.Zero, -T.One);
            public static Vector3<T> Forward   => new(T.Zero, T.Zero, T.One);
            public static Vector3<T> OneOneOne => new(T.One);


            public int Radix => T.Radix;


            public static Vector3<T> operator ++(Vector3<T> A)
            {
                return new Vector3<T>(A.X + T.One, A.Y + T.One, A.Z + T.One);
            }

            public static Vector3<T> operator --(Vector3<T> A)
            {
                return new Vector3<T>(A.X - T.One, A.Y - T.One, A.Z - T.One);
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


            public Vector3<T> Abs()
            {
                return new Vector3<T>(T.Abs(Value.X), T.Abs(Value.Y), T.Abs(Value.Z));
            }


            public bool IsZero()
            {
                return Value.X == T.Zero && Value.Y == T.Zero && Value.Z == T.Zero;
            }

            public bool IsPositive()
            {
                return T.IsPositive(Value.X) && T.IsPositive(Value.Y) && T.IsPositive(Value.Z);
            }

            public bool IsNegative()
            {
                return T.IsNegative(Value.X) && T.IsNegative(Value.Y) && T.IsNegative(Value.Z);
            }


            public bool IsNormal()
            {
                return T.IsNormal(Value.X) && T.IsNormal(Value.Y) && T.IsNormal(Value.Z);
            }

            public bool IsSubnormal()
            {
                return T.IsSubnormal(Value.X) && T.IsSubnormal(Value.Y) && T.IsSubnormal(Value.Z);
            }


            public bool IsFinite()
            {
                return T.IsFinite(Value.X) && T.IsFinite(Value.Y) && T.IsFinite(Value.Z);
            }

            public bool IsInfinity()
            {
                return T.IsInfinity(Value.X) && T.IsInfinity(Value.Y) && T.IsInfinity(Value.Z);
            }


            public bool IsNaN()
            {
                return T.IsNaN(Value.X) && T.IsNaN(Value.Y) && T.IsNaN(Value.Z);
            }

            public bool IsCanonical()
            {
                return T.IsCanonical(Value.X) && T.IsCanonical(Value.Y) && T.IsCanonical(Value.Z);
            }

            public bool IsComplexNumber()
            {
                return T.IsComplexNumber(Value.X) && T.IsComplexNumber(Value.Y) && T.IsComplexNumber(Value.Z);
            }

            public bool IsRealNumber()
            {
                return T.IsRealNumber(Value.X) && T.IsRealNumber(Value.Y) && T.IsRealNumber(Value.Z);
            }

            public bool IsImaginaryNumber()
            {
                return T.IsImaginaryNumber(Value.X) && T.IsImaginaryNumber(Value.Y) && T.IsImaginaryNumber(Value.Z);
            }


            public bool IsEvenInteger()
            {
                return T.IsEvenInteger(Value.X) && T.IsEvenInteger(Value.Y) && T.IsEvenInteger(Value.Z);
            }

            public bool IsOddInteger()
            {
                return T.IsOddInteger(Value.X) && T.IsOddInteger(Value.Y) && T.IsOddInteger(Value.Z);
            }

            public bool IsInteger()
            {
                return T.IsInteger(Value.X) && T.IsInteger(Value.Y) && T.IsInteger(Value.Z);
            }


            public bool IsPositiveInfinity()
            {
                return T.IsPositiveInfinity(Value.X) && T.IsPositiveInfinity(Value.Y) && T.IsPositiveInfinity(Value.Z);
            }

            public bool IsNegativeInfinity()
            {
                return T.IsNegativeInfinity(Value.X) && T.IsNegativeInfinity(Value.Y) && T.IsNegativeInfinity(Value.Z);
            }


            public static Vector3<T> MaxMagnitude(Vector3<T> A, Vector3<T> B)
            {
                return new Vector3<T>(
                    T.MaxMagnitude(A.X, B.X), T.MaxMagnitude(A.Y, B.Y), T.MaxMagnitude(A.Z, B.Z)
                );
            }

            public static Vector3<T> MaxMagnitudeNumber(Vector3<T> A, Vector3<T> B)
            {
                return new Vector3<T>(
                    T.MaxMagnitudeNumber(A.X, B.X), T.MaxMagnitudeNumber(A.Y, B.Y), T.MaxMagnitudeNumber(A.Z, B.Z)
                );
            }

            public static Vector3<T> MinMagnitude(Vector3<T> A, Vector3<T> B)
            {
                return new Vector3<T>(
                    T.MinMagnitude(A.X, B.X), T.MinMagnitude(A.Y, B.Y), T.MinMagnitude(A.Z, B.Z)
                );
            }

            public static Vector3<T> MinMagnitudeNumber(Vector3<T> A, Vector3<T> B)
            {
                return new Vector3<T>(
                    T.MinMagnitudeNumber(A.X, B.X), T.MinMagnitudeNumber(A.Y, B.Y), T.MinMagnitudeNumber(A.Z, B.Z)
                );
            }
        }

        extension<T>(Vector3<T> Value) where T : IRootFunctions<T>
        {
            public T Magnitude => T.Sqrt(Value.LengthSquared);

            public T LengthSquared
            {
                get
                {
                    var X = Value.X;
                    var Y = Value.Y;
                    var Z = Value.Z;
                    return X * X + Y * Y + Z * Z;
                }
            }

            public Vector3<T> Normal
            {
                get
                {
                    var Zero = Vector3<T>.Zero;
                    if (Value == Zero)
                    {
                        return Zero;
                    }

                    var Magnitude = Value.Magnitude;
                    return new Vector3<T>(Value.X / Magnitude, Value.Y / Magnitude, Value.Z / Magnitude);
                }
            }


            public static T Distance(Vector3<T> A, Vector3<T> B)
            {
                return (B - A).Magnitude;
            }
        }
        #endregion
        #endregion
    }
}