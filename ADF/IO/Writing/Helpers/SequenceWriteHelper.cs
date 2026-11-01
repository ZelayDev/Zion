using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Zion.Geometry;
using Vector2 = Zion.Geometry.Vector2;
using Vector3 = Zion.Geometry.Vector3;

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
                    Stream.Write7BitEncoded(Registry.GetOrAdd(Value));
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

        public static void Write<T>(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<T> Span)
        {
            if (TryWriteReference(Context, Target, Collection))
            {
                return;
            }

            var Stream = GetSubStream(Context, Target, Span.Length, Span.Length * 5);

            //TODO: SequenceWriteHelper.Write<T>

            Target.Add(Stream);
        }


        public static void Write(ArenaStream Stream, ReadOnlySpan<bool> Span, bool Compression)
        {
            Stream.WriteCompressed(Span);
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<byte> Span, bool Compression)
        {
            Stream.Write(Span);
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<sbyte> Span, bool Compression)
        {
            Stream.Write(MemoryMarshal.AsBytes(Span));
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<short> Span, bool Compression)
        {
            Stream.Write(Span);
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<int> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                Stream.Write(Span);
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
                Stream.Write(Span);
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<ushort> Span, bool Compression)
        {
            Stream.Write(Span);
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<uint> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                Stream.Write(Span);
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
                Stream.Write(Span);
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<char> Span, bool Compression)
        {
            Stream.Write(Span);
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<float> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, Span);
            }
            else
            {
                Stream.Write(Span);
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
                Stream.Write(Span);
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
                Stream.Write(Span);
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Half> Span, bool Compression)
        {
            Stream.Write(Span);
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Index> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Index, int>(Span));
            }
            else
            {
                Stream.Write(Span);
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
                Stream.Write(Span);
            }
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<RGBColor> Span, bool Compression)
        {
            Stream.Write(Span);
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<RGBAColor> Span, bool Compression)
        {
            Stream.Write(Span);
        }

        public static void Write(ArenaStream Stream, ReadOnlySpan<Vector2> Span, bool Compression)
        {
            if (Compression)
            {
                WriteCompressed(Stream, MemoryMarshal.Cast<Vector2, float>(Span));
            }
            else
            {
                Stream.Write(Span);
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
                Stream.Write(Span);
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
                Stream.Write(Span);
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
                Stream.Write(Span);
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
            var Capacity = Span.Length * 5;
            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                (Target, Source) =>
                {
                    var Writer = new VarIntWriter(Stream, Target);

                    foreach (var Value in Source)
                    {
                        Writer.WriteVarInt(Value);
                    }

                    Writer.Complete();
                }
            );
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<uint> Span)
        {
            var Capacity = Span.Length * 5;
            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                (Target, Source) =>
                {
                    var Writer = new VarIntWriter(Stream, Target);

                    foreach (var Value in Source)
                    {
                        Writer.WriteVarInt(Value);
                    }

                    Writer.Complete();
                }
            );
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<long> Span)
        {
            var Capacity = Span.Length * 10;
            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                (Target, Source) =>
                {
                    var Writer = new VarIntWriter(Stream, Target);

                    foreach (var Value in Source)
                    {
                        Writer.WriteVarInt(Value);
                    }

                    Writer.Complete();
                }
            );
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<ulong> Span)
        {
            var Capacity = Span.Length * 10;
            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                (Target, Source) =>
                {
                    var Writer = new VarIntWriter(Stream, Target);

                    foreach (var Value in Source)
                    {
                        Writer.WriteVarInt(Value);
                    }

                    Writer.Complete();
                }
            );
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<float> Span)
        {
            var Capacity = Span.Length * 5;
            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                (Target, Source) =>
                {
                    var Writer = new VarIntWriter(Stream, Target);
                    var Previous = 0u;

                    foreach (var Value in Source)
                    {
                        var Raw = Unsafe.As<float, uint>(ref Unsafe.AsRef(in Value));
                        Writer.WriteVarInt(Raw ^ Previous);
                        Previous = Raw;
                    }

                    Writer.Complete();
                }
            );
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<double> Span)
        {
            var Capacity = Span.Length * 10;
            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                (Target, Source) =>
                {
                    var Writer = new VarIntWriter(Stream, Target);
                    var Previous = 0L;

                    foreach (var Value in Source)
                    {
                        var Raw = Unsafe.As<double, long>(ref Unsafe.AsRef(in Value));
                        Writer.WriteVarInt(Raw ^ Previous);
                        Previous = Raw;
                    }

                    Writer.Complete();
                }
            );
        }

        private static void WriteCompressed(ArenaStream Stream, ReadOnlySpan<decimal> Span)
        {
            var Capacity = Span.Length * sizeof(decimal);
            Stream.Reserve(Capacity);
            Stream.UseSpan
            (
                Capacity,
                Span,
                (Target, Source) =>
                {
                    var Writer = new VarIntWriter(Stream, Target);
                    Span<int> Bits = stackalloc int[4];

                    foreach (var Value in Source)
                    {
                        Decimal.GetBits(Value, Bits);

                        var Lo = unchecked((uint)Bits[0]);
                        var Mid = unchecked((uint)Bits[1]);
                        var Hi = unchecked((uint)Bits[2]);
                        var Flags = unchecked((uint)Bits[3]);

                        var Sign = (Flags & 0x80000000u) != 0;
                        var Scale = (byte)((Flags >> 16) & 0xFF);
                        var SignScale = (byte)((Sign ? 0x80 : 0) | (Scale & 0x7F));

                        Writer.WriteByte(SignScale);

                        var Low64 = Lo | ((ulong)Mid << 32);
                        Writer.WriteVarInt(Low64);
                        Writer.WriteVarInt(Hi);
                    }

                    Writer.Complete();
                }
            );
        }


        private ref struct VarIntWriter
        {
            private readonly ArenaStream                Stream;
            private readonly ArenaChunkEnumerator<byte> Chunks;

            private Span<byte> Current;
            private int        Index;
            private int        Written;


            public VarIntWriter(ArenaStream Stream, ArenaSpan<byte> Target)
            {
                this.Stream = Stream;

                Chunks = Target.Chunks.GetEnumerator();
                Chunks.MoveNext();
                Current = Chunks.Current;

                Index = 0;
                Written = 0;
            }

            
            public void WriteByte(byte Value)
            {
                EnsureSpace(1);
                Current[Index++] = Value;
                Written++;
            }

            public void WriteVarInt(int Value)
            {
                WriteVarInt((uint)((Value << 1) ^ (Value >> 31)));
            }

            public void WriteVarInt(uint Value)
            {
                while (Value >= 0x80)
                {
                    WriteByte((byte)(Value | 0x80));
                    Value >>= 7;
                }
                WriteByte((byte)Value);
            }

            public void WriteVarInt(long Value)
            {
                WriteVarInt((ulong)((Value << 1) ^ (Value >> 63)));
            }

            public void WriteVarInt(ulong Value)
            {
                while (Value >= 0x80)
                {
                    WriteByte((byte)(Value | 0x80));
                    Value >>= 7;
                }
                WriteByte((byte)Value);
            }
            
            
            private void EnsureSpace(int Size)
            {
                while (Index + Size > Current.Length)
                {
                    Current = Chunks.Current;
                    Index = 0;
                }
            }

            
            public void Complete()
            {
                Stream.TrimExcess(Written);
            }
        }
    }
}