namespace Zion.Serialization
{
    public interface IBinaryWritable
    {
        /// <summary>
        ///     Serializes the object to binary stream.
        /// </summary>
        public void Write(BinaryWriter Writer);
    }

    /// <summary>
    ///     Serializes the object to binary stream.
    /// </summary>
    public interface IBinaryWritable<T>
    {
        /// <summary>
        ///     Serializes the object to binary stream.
        /// </summary>
        public void Write(BinaryWriter Writer, Action<T> Write);
    }
}