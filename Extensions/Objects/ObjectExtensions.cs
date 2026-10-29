namespace Zion
{
    public static class ObjectExtensions
    {
        extension(object Object)
        {
            /// <summary>
            /// Converts an object to a colored string representation if it implements IColorText,
            /// otherwise returns the standard string representation.
            /// </summary>
            /// <param name="Object">The object to convert to string (can be null)</param>
            /// <param name="Nullable">The string to return if the object is null (default: "null")</param>
            /// <returns>
            /// Colored text if object implements IColorText, standard string representation otherwise,
            /// or the nullable replacement string for null objects.
            /// </returns>
            /// <remarks>
            /// If the object implements IColorText, the method calls its ToColorString() implementation.
            /// For non-IColorText objects, it falls back to standard ToString() behavior.
            /// </remarks>
            public string ToColorString(string Nullable = "null")
            {
                return Object is IColorText Colorable
                    ? Object?.ToColorString() ?? Nullable ?? "null"
                    : ToNotNullString(Object, Nullable);
            }


            /// <summary>
            /// Converts an object to its string representation, ensuring a non-null result.
            /// Uses "null" as the default replacement for null objects.
            /// </summary>
            /// <param name="Object">The object to convert to string (can be null)</param>
            /// <returns>
            /// The object's string representation or "null" if the object is null.
            /// </returns>
            public string ToNotNullString()
            {
                return ToNotNullString(Object, "null");
            }

            /// <summary>
            /// Converts an object to its string representation with a custom replacement for null objects.
            /// </summary>
            /// <param name="Object">The object to convert to string (can be null)</param>
            /// <param name="Nullable">The custom string to return if the object is null</param>
            /// <returns>
            /// The object's string representation or the specified nullable replacement string.
            /// </returns>
            public string ToNotNullString(string Nullable = "null")
            {
                return Object?.ToString() ?? Nullable ?? "null";
            }


            public static void Swap<T>(ref T A, ref T B)
            {
                T Temp = A;
                A = B;
                B = Temp;
            }

            public static void Swap<T>(ref T A, ref T B, ref T Temp)
            {
                Temp = A;
                A = B;
                B = Temp;
            }


            public static bool CompareReferences<T>(T? A, T? B, out bool Result) where T : class
            {
                if (ReferenceEquals(A, B))
                {
                    Result = true;
                    return true;
                }

                if (A is null || B is null)
                {
                    Result = false;
                    return true;
                }

                Result = false;
                return false;
            }
        }
    }
}