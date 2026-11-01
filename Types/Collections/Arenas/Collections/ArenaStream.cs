using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Zion.Geometry;
using static System.Buffers.Binary.BinaryPrimitives;
using static System.Runtime.InteropServices.MemoryMarshal;
using Vector2 = Zion.Geometry.Vector2;
using Vector3 = Zion.Geometry.Vector3;

namespace Zion
{
    public sealed class ArenaStream : ArenaCollection<byte>
    {
        private const int BufferThreshold = 256;

        public int Position { get; private set; }

        public int Length
        {
            get;
            private set
            {
                field = value;
                UpdateLimit(value);
            }
        }


        public byte this[int Index]
        {
            get => GetValueAt(Index);
            set => SetValueAt(Index, value);
        }

        public byte this[Index Index]
        {
            get => GetValueAt(Index);
            set => SetValueAt(Index, value);
        }


        public void Write(bool Value)
        {
            Write(Value ? (byte)1 : (byte)0);
        }

        public void Write(byte Value)
        {
            Reserve(1);
            this[Position] = Value;
        }

        public void Write(sbyte Value)
        {
            Write((byte)Value);
        }

        public void Write(int Value)
        {
            const int Size = sizeof(int);

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteInt32LittleEndian(Span, Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteInt32LittleEndian(Buffer, Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(uint Value)
        {
            const int Size = sizeof(uint);

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteUInt32LittleEndian(Span, Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteUInt32LittleEndian(Buffer, Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(long Value)
        {
            const int Size = sizeof(long);

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteInt64LittleEndian(Span, Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteInt64LittleEndian(Buffer, Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(ulong Value)
        {
            const int Size = sizeof(ulong);

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteUInt64LittleEndian(Span, Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteUInt64LittleEndian(Buffer, Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(short Value)
        {
            const int Size = sizeof(short);

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteInt16LittleEndian(Span, Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteInt16LittleEndian(Buffer, Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(ushort Value)
        {
            const int Size = sizeof(ushort);

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteUInt16LittleEndian(Span, Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteUInt16LittleEndian(Buffer, Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(float Value)
        {
            const int Size = sizeof(float);

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteSingleLittleEndian(Span, Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteSingleLittleEndian(Buffer, Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(double Value)
        {
            const int Size = sizeof(double);

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteDoubleLittleEndian(Span, Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteDoubleLittleEndian(Buffer, Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(decimal Value)
        {
            const int Size = sizeof(decimal);

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                Decimal.GetBits(Value, Cast<byte, int>(Span));
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];

                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(char Value)
        {
            Write((ushort)Value);
        }

        public void Write(string Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

            if (Value.Length == 0)
            {
                Write7BitEncoded(0);
                return;
            }

            var MaxLength = Value.Length * 4 + 5;

            if (TryGetSpan(Position, MaxLength, out var Span))
            {
                var StartPosition = Position;
                var Length = Encoding.UTF8.GetByteCount(Value);

                Write7BitEncoded(Length);
                Reserve(Length);

                Span = Span.Slice(Position - StartPosition, Length);

                Encoding.UTF8.GetBytes(Value, Span);
                Advance(Length);
            }
            else if (MaxLength <= BufferThreshold)
            {
                Span<byte> Buffer = stackalloc byte[MaxLength];

                var Length = Encoding.UTF8.GetBytes(Value, Buffer);

                Write7BitEncoded(Length);
                Reserve(Length);

                Buffer.Slice(0, Length).CopyTo(AsSpan(Position, Length));
                Advance(Length);
            }
            else
            {
                var Length = Encoding.UTF8.GetByteCount(Value);
                var Encoder = Encoding.UTF8.GetEncoder();

                ReadOnlySpan<char> Chars = Value;

                Write7BitEncoded(Length);
                Reserve(Length);

                foreach (var Chunk in AsSpan(Position, Length).Chunks)
                {
                    Encoder.Convert(Chars, Chunk, false, out var CharsUsed, out _, out _);
                    Chars = Chars.Slice(CharsUsed);
                    if (Chars.IsEmpty)
                    {
                        break;
                    }
                }

                if (!Chars.IsEmpty)
                {
                    Encoder.Convert(Chars, Span<byte>.Empty, true, out _, out _, out _);
                }

                Advance(Length);
            }
        }


        public void Write(Half Value)
        {
            const int Size = 2;

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteHalfLittleEndian(Span, Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteHalfLittleEndian(Buffer, Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(Index Value)
        {
            Write(Unsafe.As<Index, int>(ref Value));
        }

        public void Write(Range Value)
        {
            const int Size = 8;

            var Start = Value.Start;
            var End = Value.End;

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                WriteInt32LittleEndian(Span, Unsafe.As<Index, int>(ref Start));
                WriteInt32LittleEndian(Span.Slice(4), Unsafe.As<Index, int>(ref End));
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                WriteInt32LittleEndian(Buffer, Unsafe.As<Index, int>(ref Start));
                WriteInt32LittleEndian(Buffer.Slice(4), Unsafe.As<Index, int>(ref End));
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(BigInteger Value)
        {
            var Length = Value.GetByteCount();

            Write7BitEncoded(Length);
            Reserve(Length);

            if (TryGetSpan(Position, Length, out var Span))
            {
                Value.TryWriteBytes(Span, out _);
                return;
            }

            var Destination = AsSpan(Position, Length);

            if (Length <= BufferThreshold)
            {
                Span<byte> Buffer = stackalloc byte[Length];

                Value.TryWriteBytes(Buffer, out _);
                Buffer.CopyTo(Destination);
            }
            else
            {
                var Buffer = ArrayPool<byte>.Shared.Rent(Length);

                Value.TryWriteBytes(Buffer, out _);
                Buffer.CopyTo(Destination);

                ArrayPool<byte>.Shared.Return(Buffer);
            }

            Advance(Length);
        }


        public void Write(RGBColor Value)
        {
            const int Size = 3;
            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                MemoryMarshal.Write(Span, in Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                MemoryMarshal.Write(Buffer, in Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(RGBAColor Value)
        {
            const int Size = 4;
            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                MemoryMarshal.Write(Span, in Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                MemoryMarshal.Write(Buffer, in Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }


        public void Write(Vector2 Value)
        {
            const int Size = 8;
            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                MemoryMarshal.Write(Span, in Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                MemoryMarshal.Write(Buffer, in Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(Vector2Int Value)
        {
            const int Size = 8;
            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                MemoryMarshal.Write(Span, in Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                MemoryMarshal.Write(Buffer, in Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(Vector2<byte> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<sbyte> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<int> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<uint> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<long> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<ulong> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<short> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<ushort> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<float> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<double> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector2<decimal> Value)
        {
            WriteVector(Value);
        }


        public void Write(Vector3 Value)
        {
            const int Size = 12;
            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                MemoryMarshal.Write(Span, in Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                MemoryMarshal.Write(Buffer, in Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(Vector3Int Value)
        {
            const int Size = 12;
            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                MemoryMarshal.Write(Span, in Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                MemoryMarshal.Write(Buffer, in Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        public void Write(Vector3<byte> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<sbyte> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<int> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<uint> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<long> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<ulong> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<short> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<ushort> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<float> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<double> Value)
        {
            WriteVector(Value);
        }

        public void Write(Vector3<decimal> Value)
        {
            WriteVector(Value);
        }


        public void Write(ArenaSpan<byte> Span)
        {
            Reserve(Span.Length);
            Span.CopyTo(AsSpan(Position, Span.Length));
        }

        public void Write(ArenaStream Stream)
        {
            Reserve(Stream.Length);

            var Source = Stream.AsSpan();
            var Destination = AsSpan(Position, Source.Length);

            Source.CopyTo(Destination);
        }


        public void Write(ReadOnlySpan<bool> Span)
        {
            Write(AsBytes(Span));
        }

        public void Write(ReadOnlySpan<byte> Span)
        {
            Reserve(Span.Length + 4);
            Span.CopyTo(AsSpan(Position, Span.Length));
        }

        public void Write(ReadOnlySpan<sbyte> Span)
        {
            Write(Cast<sbyte, byte>(Span));
        }

        public void Write(ReadOnlySpan<short> Span)
        {
            WriteSpan(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<int> Span)
        {
            WriteSpan(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<long> Span)
        {
            WriteSpan(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<ushort> Span)
        {
            WriteSpan(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<uint> Span)
        {
            WriteSpan(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<ulong> Span)
        {
            WriteSpan(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<char> Span)
        {
            WriteSpan<char, short>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<float> Span)
        {
            WriteSpan<float, int>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<double> Span)
        {
            WriteSpan<double, long>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<decimal> Span)
        {
            WriteSpan<decimal, int>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<Half> Span)
        {
            WriteSpan<Half, short>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<Index> Span)
        {
            WriteSpan<Index, int>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<Range> Span)
        {
            WriteSpan<Range, int>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<RGBColor> Span)
        {
            Write(AsBytes(Span));
        }

        public void Write(ReadOnlySpan<RGBAColor> Span)
        {
            Write(AsBytes(Span));
        }

        public void Write(ReadOnlySpan<Vector2> Span)
        {
            WriteSpan<Vector2, int>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<Vector2Int> Span)
        {
            WriteSpan<Vector2Int, int>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<Vector3> Span)
        {
            WriteSpan<Vector3, int>(Span, ReverseEndianness);
        }

        public void Write(ReadOnlySpan<Vector3Int> Span)
        {
            WriteSpan<Vector3Int, int>(Span, ReverseEndianness);
        }


        public void WriteCompressed(ReadOnlySpan<bool> Span)
        {
            var ByteCount = (Span.Length + 7) >> 3;
            var FullBytes = Span.Length >> 3;
            var Remainder = Span.Length & 0b111;

            Reserve(ByteCount);
            var Destination = AsSpan(Position, ByteCount);
            Advance(ByteCount);

            var SourceIndex = 0;
            var ByteIndex = 0;

            foreach (var Chunk in Destination.Chunks)
            {
                for (var i = 0; i < Chunk.Length; i++)
                {
                    if (ByteIndex >= ByteCount)
                    {
                        return;
                    }

                    byte Byte = 0;

                    if (ByteIndex < FullBytes)
                    {
                        for (var bit = 0; bit < 8; bit++, SourceIndex++)
                        {
                            if (Span[SourceIndex])
                            {
                                Byte |= (byte)(1 << bit);
                            }
                        }
                    }
                    else if (ByteIndex == FullBytes)
                    {
                        for (var bit = 0; bit < Remainder; bit++, SourceIndex++)
                        {
                            if (Span[SourceIndex])
                            {
                                Byte |= (byte)(1 << bit);
                            }
                        }
                    }

                    Chunk[i] = Byte;
                    ByteIndex++;
                }
            }
        }


        public void Write7BitEncoded(int Value)
        {
            Write7BitEncoded((uint)((Value << 1) ^ (Value >> 31)));
        }

        public void Write7BitEncoded(long Value)
        {
            Write7BitEncoded((ulong)((Value << 1) ^ (Value >> 63)));
        }

        public void Write7BitEncoded(uint Value)
        {
            const int Size = 5;

            Reserve(Size);

            var Index = 0;

            if (TryGetSpan(Position, Size, out var Span))
            {
                while (Value >= 0x80)
                {
                    Span[Index++] = (byte)(Value | 0x80);
                    Value >>= 7;
                }
                Span[Index++] = (byte)Value;
            }
            else
            {
                var ArenaSpan = AsSpan(Position, Size);

                while (Value >= 0x80)
                {
                    ArenaSpan[Index++] = (byte)(Value | 0x80);
                    Value >>= 7;
                }
                ArenaSpan[Index++] = (byte)Value;
            }

            Advance(Index);
        }

        public void Write7BitEncoded(ulong Value)
        {
            const int Size = 10;

            Reserve(Size);

            var Index = 0;

            if (TryGetSpan(Position, Size, out var Span))
            {
                while (Value >= 0x80)
                {
                    Span[Index++] = (byte)(Value | 0x80);
                    Value >>= 7;
                }
                Span[Index++] = (byte)Value;
            }
            else
            {
                var ArenaSpan = AsSpan(Position, Size);

                while (Value >= 0x80)
                {
                    ArenaSpan[Index++] = (byte)(Value | 0x80);
                    Value >>= 7;
                }
                ArenaSpan[Index++] = (byte)Value;
            }

            Advance(Index);
        }

        public void Write7BitEncoded(Index Value)
        {
            Write7BitEncoded(Unsafe.As<Index, uint>(ref Value));
        }


        public void WriteEnum<T>(T Value) where T : struct, Enum
        {
            var Size = Unsafe.SizeOf<T>();

            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                MemoryMarshal.Write(Span, in Value);
                return;
            }

            var Destination = AsSpan(Position, Size);

            if (Size <= BufferThreshold)
            {
                Span<byte> Buffer = stackalloc byte[Size];
                MemoryMarshal.Write(Buffer, in Value);
                Buffer.CopyTo(Destination);
            }
            else
            {
                var Buffer = ArrayPool<byte>.Shared.Rent(Length);

                MemoryMarshal.Write(Buffer, in Value);
                Buffer.AsSpan().Slice(0, Size).CopyTo(Destination);

                ArrayPool<byte>.Shared.Return(Buffer);
            }
        }


        public void SetPosition(int Position)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Position);
            this.Position = Position;
        }


        public byte[] ToArray()
        {
            return AsSpan().ToArray();
        }


        public new void UseSpan(Action<ArenaSpan<byte>> Action)
        {
            base.UseSpan(Position, Action);
            Position = Length;
        }

        public new void UseSpan(int Count, Action<ArenaSpan<byte>> Action)
        {
            base.UseSpan(Position, Count, Action);
            Advance(Count);
        }

        public new void UseSpan(int Start, int Count, Action<ArenaSpan<byte>> Action)
        {
            EnsureCapacity(Start + Count);
            base.UseSpan(Start, Count, Action);
        }


        public new void UseSpan<I>(I Other, Action<ArenaSpan<byte>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Position, Other, Action);
            Position = Length;
        }

        public new void UseSpan<I>(int Count, I Other, Action<ArenaSpan<byte>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Position, Count, Other, Action);
            Advance(Count);
        }

        public new void UseSpan<I>(int Start, int Count, I Other, Action<ArenaSpan<byte>, I> Action)
            where I : allows ref struct
        {
            EnsureCapacity(Start + Count);
            base.UseSpan(Start, Count, Other, Action);
        }


        public new R UseSpan<R>(Func<ArenaSpan<byte>, R> Function)
        {
            var Result = base.UseSpan(Position, Function);
            Position = Length;
            return Result;
        }

        public new R UseSpan<R>(int Count, Func<ArenaSpan<byte>, R> Function)
        {
            var Result = base.UseSpan(Position, Count, Function);
            Advance(Count);
            return Result;
        }

        public new R UseSpan<R>(int Start, int Count, Func<ArenaSpan<byte>, R> Function)
        {
            EnsureCapacity(Start + Count);
            return base.UseSpan(Start, Count, Function);
        }


        public new R UseSpan<I, R>(I Other, Func<ArenaSpan<byte>, I, R> Function) where I : allows ref struct
        {
            var Result = base.UseSpan(Other, Function);
            Position = Length;
            return Result;
        }

        public new R UseSpan<I, R>(int Count, I Other, Func<ArenaSpan<byte>, I, R> Function) where I : allows ref struct
        {
            var Result = base.UseSpan(Position, Count, Other, Function);
            Advance(Count);
            return Result;
        }

        public new R UseSpan<I, R>(int Start, int Count, I Other, Func<ArenaSpan<byte>, I, R> Function)
            where I : allows ref struct
        {
            EnsureCapacity(Start + Count);
            return base.UseSpan(Start, Count, Other, Function);
        }


        public void CopyTo(Span<byte> Destination)
        {
            AsSpan().CopyTo(Destination);
        }

        public void CopyTo(ArenaSpan<byte> Destination)
        {
            AsSpan().CopyTo(Destination);
        }

        public void CopyTo(Stream Destination)
        {
            foreach (var Span in AsSpan().Chunks)
            {
                Destination.Write(Span);
            }
        }

        public void CopyTo(ArenaStream Destination)
        {
            Destination.EnsureCapacity(Length);

            var SourceSpan = AsSpan();
            var DestinationSpan = Destination.AsSpan(0, SourceSpan.Length);

            SourceSpan.CopyTo(DestinationSpan);
        }


        public void Reserve(int Capacity)
        {
            EnsureCapacity(Position + Capacity);
        }

        public void EnsureCapacity(int Capacity)
        {
            Extend(Capacity);
        }

        public void TrimExcess(int Size)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Size);
            
            if (Size < Length)
            {
                Length = Size;
                Position = Size;
                Trim(Size);
            }
        }

        public void TrimExcess()
        {
            Trim(Length);
        }


        public void Advance(int Offset)
        {
            var NewPosition = Position += Offset;
            if (NewPosition > Length)
            {
                Length = NewPosition;
            }
        }

        private void WriteVector<T>(Vector2<T> Value) where T : unmanaged, INumber<T>
        {
            var Size = Vector2<T>.GetByteCount();
            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                MemoryMarshal.Write(Span, in Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                MemoryMarshal.Write(Buffer, in Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }

        private void WriteVector<T>(Vector3<T> Value) where T : unmanaged, INumber<T>
        {
            var Size = Vector3<T>.GetByteCount();
            Reserve(Size);

            if (TryGetSpan(Position, Size, out var Span))
            {
                MemoryMarshal.Write(Span, in Value);
            }
            else
            {
                Span<byte> Buffer = stackalloc byte[Size];
                MemoryMarshal.Write(Buffer, in Value);
                Buffer.CopyTo(AsSpan(Position, Size));
            }

            Advance(Size);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpan<T, TCast>(ReadOnlySpan<T> Span, Action<ReadOnlySpan<TCast>, Span<TCast>> ReverseEndianness) where T : unmanaged where TCast : unmanaged
        {
            WriteSpan<TCast>(Cast<T, TCast>(Span), ReverseEndianness);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSpan<T>(ReadOnlySpan<T> Span, Action<ReadOnlySpan<T>, Span<T>> ReverseEndianness) where T : unmanaged
        {
            if (BitConverter.IsLittleEndian)
            {
                Write(AsBytes(Span));
            }
            else
            {
                var Size = Span.Length * Unsafe.SizeOf<T>();
                Reserve(Size);
                if (TryGetSpan(Position, Size, out var Destination))
                {
                    ReverseEndianness(Span, Cast<byte, T>(Destination));
                }
                //TODO
                Advance(Size);
            }
        }
    }
}