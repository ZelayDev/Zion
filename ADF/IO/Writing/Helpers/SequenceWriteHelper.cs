using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Zion.Vectors;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion.ADF
{
    internal static class SequenceWriteHelper
    {
        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<bool> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<byte> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<sbyte> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<short> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<int> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<long> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<ushort> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<uint> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<ulong> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<char> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<float> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<double> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<decimal> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<string> Span)
        {
            if (TryWriteReference(Context, Target, Collection))
            {
                return;
            }

            var Registry = Context.Registries.StringRegistry;
            var Stream = GetSubStream(Context, Target, Span.Length, Span.Length * 4);

            if (Context.Compression)
            {
                foreach (var Value in Span)
                {
                    Stream.Write7BitEncodedUInt(Registry.GetOrAdd(Value));
                }
            }
            else
            {
                foreach (var Value in Span)
                {
                    Stream.Write(Registry.GetOrAdd(Value));
                }
            }

            Target.Add(Stream);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<Half> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<Index> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<Range> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<BigInteger> Span)
        {
            if (TryWriteReference(Context, Target, Collection))
            {
                return;
            }

            var Stream = GetSubStream(Context, Target, Span.Length, Span.Length * 5);

            foreach (var Value in Span)
            {
                Stream.Write(Value);
            }

            Target.Add(Stream);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<RGBColor> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<RGBAColor> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<Vector2> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<Vector2Int> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<Vector3> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<Vector3Int> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<System.Numerics.Vector2> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<System.Numerics.Vector3> Span)
        {
            Write(Context, Target, Collection, Span, Write);
        }

        public static void Write<T>(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<T> Span)
        {
            if (TryWriteReference(Context, Target, Collection))
            {
                return;
            }

            var Stream = GetSubStream(Context, Target, Span.Length, Span.Length * 5);

            //TODO: SequenceWriteHelper.Write<Node>

            Target.Add(Stream);
        }


        public static void Write(ArenaStream Stream, ReadOnlySpan<bool> Span, bool Compression)
        {
            var Capacity = (Span.Length + 7) >> 3;

            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                (Target, Source) =>
                {
                    int Count = Source.Length;
                    int ByteCount = (Count + 7) >> 3;

                    int ByteIndex = 0;
                    int BitIndex = 0;

                    byte Current = 0;

                    for (int i = 0; i < Count; i++)
                    {
                        if (Source[i])
                        {
                            Current |= (byte)(1 << BitIndex);
                        }

                        BitIndex++;

                        if (BitIndex == 8)
                        {
                            Target[ByteIndex++] = Current;
                            Current = 0;
                            BitIndex = 0;
                        }
                    }

                    if (BitIndex > 0)
                    {
                        Target[ByteIndex] = Current;
                    }
                }
            );
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<byte> Span, bool Compression)
        {
            Stream.Reserve(Span.Length);
            Stream.UseSpan
            (
                Span.Length,
                Span,
                static (Target, Source) =>
                {
                    Source.CopyTo(Target);
                }
            );
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<sbyte> Span, bool Compression)
        {
            Stream.Reserve(Span.Length);
            Stream.UseSpan
            (
                Span.Length,
                Span,
                static (Target, Source) =>
                {
                    MemoryMarshal.AsBytes(Source).CopyTo(Target);
                }
            );
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<short> Span, bool Compression)
        {
            int Capacity = Span.Length * sizeof(short);

            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                static (Target, Source) =>
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                    }
                    else
                    {
                        BinaryPrimitives.ReverseEndianness
                        (
                            Source,
                            MemoryMarshal.Cast<byte, short>(Target)
                        );
                    }
                }
            );            
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<int> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                int Capacity = Span.Length * sizeof(int);

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                Source,
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<long> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                int Capacity = Span.Length * sizeof(long);

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                Source,
                                MemoryMarshal.Cast<byte, long>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<ushort> Span, bool Compression)
        {
            int Capacity = Span.Length * sizeof(ushort);

            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                static (Target, Source) =>
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                    }
                    else
                    {
                        BinaryPrimitives.ReverseEndianness
                        (
                            Source,
                            MemoryMarshal.Cast<byte, ushort>(Target)
                        );
                    }
                }
            );
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<uint> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                int Capacity = Span.Length * sizeof(uint);

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                Source,
                                MemoryMarshal.Cast<byte, uint>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<ulong> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                int Capacity = Span.Length * sizeof(ulong);

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                Source,
                                MemoryMarshal.Cast<byte, ulong>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<char> Span, bool Compression)
        {
            int Capacity = Span.Length * sizeof(char);

            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                static (Target, Source) =>
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                    }
                    else
                    {
                        BinaryPrimitives.ReverseEndianness
                        (
                            MemoryMarshal.Cast<char, short>(Source),
                            MemoryMarshal.Cast<byte, short>(Target)
                        );
                    }
                }
            );   
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<float> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                int Capacity = Span.Length * sizeof(float);

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<float, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<double> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                int Capacity = Span.Length * sizeof(double);

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<double, long>(Source),
                                MemoryMarshal.Cast<byte, long>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<decimal> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                int Capacity = Span.Length * sizeof(decimal);

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            var UIntSource = MemoryMarshal.Cast<decimal, uint>(Source);
                            var UIntTarget = MemoryMarshal.Cast<byte, uint>(Target);

                            for (int i = 0; i < UIntSource.Length; i++)
                            {
                                UIntTarget[i] = BinaryPrimitives.ReverseEndianness(UIntSource[i]);
                            }
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Half> Span, bool Compression)
        {
            int Capacity = Span.Length * 2;

            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                static (Target, Source) =>
                {
                    if (BitConverter.IsLittleEndian)
                    {
                        MemoryMarshal.AsBytes(Source).CopyTo(Target);
                    }
                    else
                    {
                        BinaryPrimitives.ReverseEndianness
                        (
                            MemoryMarshal.Cast<Half, short>(Source),
                            MemoryMarshal.Cast<byte, short>(Target)
                        );
                    }
                }
            );
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Index> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Index, int>(Span));
            }
            else
            {
                int Capacity = Span.Length * 4;

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<Index, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Range> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Range, int>(Span));
            }
            else
            {
                int Capacity = Span.Length * 8;

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<Range, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<RGBColor> Span, bool Compression)
        {
            int Capacity = Span.Length * 3;

            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                static (Target, Source) =>
                {
                    MemoryMarshal.AsBytes(Source).CopyTo(Target);
                }
            );
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<RGBAColor> Span, bool Compression)
        {
            int Capacity = Span.Length * 4;

            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                static (Target, Source) =>
                {
                    MemoryMarshal.AsBytes(Source).CopyTo(Target);
                }
            );
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector2> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Vector2, float>(Span));
            }
            else
            {
                int Capacity = Span.Length * 8;

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<Vector2, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector2Int> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Vector2Int, int>(Span));
            }
            else
            {
                int Capacity = Span.Length * 8;

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<Vector2Int, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector3> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Vector3, float>(Span));
            }
            else
            {
                int Capacity = Span.Length * 12;

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<Vector3, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector3Int> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Vector3Int, int>(Span));
            }
            else
            {
                int Capacity = Span.Length * 12;

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<Vector3Int, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<System.Numerics.Vector2> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<System.Numerics.Vector2, float>(Span));
            }
            else
            {
                int Capacity = Span.Length * 8;

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<System.Numerics.Vector2, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<System.Numerics.Vector3> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<System.Numerics.Vector3, float>(Span));
            }
            else
            {
                int Capacity = Span.Length * 12;

                Stream.Reserve(Capacity);
                Stream.UseSpan
                (
                    Capacity,
                    Span,
                    static (Target, Source) =>
                    {
                        if (BitConverter.IsLittleEndian)
                        {
                            MemoryMarshal.AsBytes(Source).CopyTo(Target);
                        }
                        else
                        {
                            BinaryPrimitives.ReverseEndianness
                            (
                                MemoryMarshal.Cast<System.Numerics.Vector3, int>(Source),
                                MemoryMarshal.Cast<byte, int>(Target)
                            );
                        }
                    }
                );
            }
        }


        public static void Write<T>(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<T> Span, StreamWriteAction<T> WriteAction)
        {
            if (TryWriteReference(Context, Target, Collection))
            {
                return;
            }

            var Stream = GetSubStream(Context, Target, Span.Length, 0);
            WriteAction(Stream, Span, Context.Compression);
            Target.Add(Stream);
        }

        public static bool TryWriteReference(ADFWritingContext Context, StreamGroup Target, object? Collection)
        {
            if (Collection is not null
                && Context.Registries.References.TryGetReference(Collection, out var Reference))
            {
                Target.BaseStream.WriteCompressed(Context, Reference.Id);
                return true;
            }
            return false;
        }

        public static ArenaStream GetSubStream(ADFWritingContext Context, StreamGroup Target, int SpanLength, int Capacity)
        {
            var Link = Reference.CreateNewReference(Target.ChildsLength);
            var Stream = Context.Arena.GetStream(Capacity + 4);

            Target.BaseStream.WriteCompressed(Context, Link);
            Stream.Write(SpanLength);

            return Stream;
        }


        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<int> Span)
        {
            var Written = 0;

            Stream.Reserve(Span.Length * 5);
            Stream.UseSpan
            (
                new NumberSpanState<int>(Span, ref Written),
                static (Target, State) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;

                    foreach (var Value in State.Source)
                    {
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Value);
                    }

                    State.Written = Written;
                }
            );

            Stream.TrimExcess(Written);
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<uint> Span)
        {
            var Written = 0;

            Stream.Reserve(Span.Length * 5);
            Stream.UseSpan
            (
                new NumberSpanState<uint>(Span, ref Written),
                static (Target, State) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;

                    foreach (var Value in State.Source)
                    {
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Value);
                    }

                    State.Written = Written;
                }
            );

            Stream.TrimExcess(Written);
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<long> Span)
        {
            var Written = 0;

            Stream.Reserve(Span.Length * 10);
            Stream.UseSpan
            (
                new NumberSpanState<long>(Span, ref Written),
                static (Target, State) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;

                    foreach (var Value in State.Source)
                    {
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Value);
                    }

                    State.Written = Written;
                }
            );

            Stream.TrimExcess(Written);
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<ulong> Span)
        {
            var Written = 0;

            Stream.Reserve(Span.Length * 10);
            Stream.UseSpan
            (
                new NumberSpanState<ulong>(Span, ref Written),
                static (Target, State) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;

                    foreach (var Value in State.Source)
                    {
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Value);
                    }

                    State.Written = Written;
                }
            );

            Stream.TrimExcess(Written);
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<float> Span)
        {
            Stream.Reserve(Span.Length * 5);

            var Written = 0;

            Stream.UseSpan
            (
                new NumberSpanState<float>(Span, ref Written),
                static (Target, Source) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written  = 0;
                    var Previous = 0u;

                    foreach (var Value in Source.Source)
                    {
                        var Raw  = Unsafe.As<float, uint>(ref Unsafe.AsRef(in Value));
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Raw ^ Previous);
                        Previous = Raw;
                    }

                    Source.Written = Written;
                }
            );

            Stream.TrimExcess(Written);
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<double> Span)
        {
            Stream.Reserve(Span.Length * 10);

            var Written = 0;

            Stream.UseSpan
            (
                new NumberSpanState<double>(Span, ref Written),
                static (Target, Source) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;
                    var Previous = 0UL;

                    foreach (var Value in Source.Source)
                    {
                        var Raw  = Unsafe.As<double, ulong>(ref Unsafe.AsRef(in Value));
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Raw ^ Previous);
                        Previous = Raw;
                    }

                    Source.Written = Written;
                }
            );

            Stream.TrimExcess(Written);
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<decimal> Span)
        {
            Stream.Reserve(Span.Length * sizeof(decimal));

            int Written = 0;

            Stream.UseSpan
            (
                new NumberSpanState<decimal>(Span, ref Written),
                static (Target, State) =>
                {
                    ref var Destination = ref MemoryMarshal.GetReference(Target);
                    var Written = 0;

                    Span<int> Bits = stackalloc int[4];

                    foreach (var Value in State.Source)
                    {
                        decimal.GetBits(Value, Bits);

                        var Lo    = unchecked((uint)Bits[0]);
                        var Mid   = unchecked((uint)Bits[1]);
                        var Hi    = unchecked((uint)Bits[2]);
                        var Flags = unchecked((uint)Bits[3]);

                        var Sign      = (Flags & 0x80000000u) != 0;
                        var Scale     = (byte)((Flags >> 16) & 0xFF);
                        var SignScale = (byte)((Sign ? 0x80 : 0) | (Scale & 0x7F));

                        Unsafe.Add(ref Destination, Written++) = SignScale;

                        var Low64 = Lo | ((ulong)Mid << 32);

                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Low64);
                        Written += WriteVarInt(ref Unsafe.Add(ref Destination, Written), Hi);
                    }

                    State.Written = Written;
                }
            );

            Stream.TrimExcess(Written);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int WriteVarInt(ref byte Destination, int Value)
        {
            return WriteVarInt(ref Destination, (uint)((Value << 1) ^ (Value >> 31)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int WriteVarInt(ref byte Destination, uint value)
        {
            if (value < 0x80)
            {
                Destination = (byte)value;
                return 1;
            }

            int i = 0;
            while (value >= 0x80)
            {
                Unsafe.Add(ref Destination, i++) = (byte)(value | 0x80);
                value >>= 7;
            }
            Unsafe.Add(ref Destination, i++) = (byte)value;
            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int WriteVarInt(ref byte Destination, ulong Value)
        {
            if (Value < 0x80)
            {
                Destination = (byte)Value;
                return 1;
            }

            int i = 0;
            while (Value >= 0x80)
            {
                Unsafe.Add(ref Destination, i++) = (byte)(Value | 0x80);
                Value >>= 7;
            }
            Unsafe.Add(ref Destination, i++) = (byte)Value;
            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int WriteVarInt(ref byte Destination, long Value)
        {
            return WriteVarInt(ref Destination, (ulong)((Value << 1) ^ (Value >> 63)));
        }


        private ref struct NumberSpanState<T> where T : unmanaged
        {
            public ReadOnlySpan<T> Source;
            public ref int Written;

            public NumberSpanState(ReadOnlySpan<T> Source, ref int Written)
            {
                this.Source = Source;
                this.Written = ref Written;
            }
        }
    }
}