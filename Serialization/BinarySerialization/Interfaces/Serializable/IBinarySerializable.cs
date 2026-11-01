namespace Zion.Serialization
{
    /// <summary>
    ///     Interface for objects that can be serialized to binary format.
    /// </summary>
    /// <typeparam name="T">Implementing type</typeparam>
    public interface IBinarySerializable<T>
        : IBinaryWritable,
            IBinaryReadable<T>
        where T : IBinarySerializable<T>
    { }

    /// <summary>
    ///     Interface for objects that can be serialized to binary format.
    /// </summary>
    /// <typeparam name="T">Implementing type</typeparam>
    public interface IBinarySerializable<T, I>
        : IBinaryWritable<I>,
            IBinaryReadable<T, I>
        where T : IBinarySerializable<T, I>
    { }
}