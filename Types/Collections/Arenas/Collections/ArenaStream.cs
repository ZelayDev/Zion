using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Zion.Geometry;
using static System.Buffers.Binary.BinaryPrimitives;
using Vector2 = System.Numerics.Vector2;
using Vector3 = Zion.Geometry.Vector3;

namespace Zion
{
    public sealed class ArenaStream : ArenaCollection<byte>
    {
        private const int BufferThreshold = 256;

        private int Position;

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
            Reserve(sizeof(int));

            Span<byte> Buffer = stackalloc byte[sizeof(int)];
            WriteInt32LittleEndian(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, sizeof(int)));

            Advance(sizeof(int));
        }

        public void Write(uint Value)
        {
            const int Size = sizeof(uint);
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            WriteUInt32LittleEndian(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(long Value)
        {
            const int Size = sizeof(long);
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            WriteInt64LittleEndian(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(ulong Value)
        {
            const int Size = sizeof(ulong);
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            WriteUInt64LittleEndian(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(short Value)
        {
            const int Size = sizeof(short);
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            WriteInt16LittleEndian(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(ushort Value)
        {
            const int Size = sizeof(ushort);
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            WriteUInt32LittleEndian(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(float Value)
        {
            const int Size = sizeof(float);
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            WriteSingleLittleEndian(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(double Value)
        {
            const int Size = sizeof(double);
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            WriteDoubleLittleEndian(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(decimal Value)
        {
            const int Size = sizeof(decimal);
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            Decimal.GetBits(Value, MemoryMarshal.Cast<byte, int>(Buffer));
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(char Value)
        {
            Write((ushort)Value);
        }

        public void Write(string Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

            var MaxLength = Value.Length * 3;

            if (MaxLength <= BufferThreshold)
            {
                Span<byte> Buffer = stackalloc byte[MaxLength];

                var Length = Encoding.UTF8.GetBytes(Value, Buffer);

                Write7BitEncoded(Length);
                Reserve(Length);

                Buffer.CopyTo(AsSpan(Position, Length));

                Advance(Length);
            }
            else
            {
                var Encoder = Encoding.UTF8.GetEncoder();
                var Length = Encoding.UTF8.GetByteCount(Value);

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

            Span<byte> Buffer = stackalloc byte[Size];
            WriteHalfLittleEndian(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(Index Value)
        {
            Write(Unsafe.As<Index, int>(ref Value));
        }

        public void Write(Range Value)
        {
            const int Size = 8;
            Reserve(Size);

            var Start = Value.Start;
            var End = Value.End;

            Span<byte> Buffer = stackalloc byte[Size];
            WriteInt32LittleEndian(Buffer, Unsafe.As<Index, int>(ref Start));
            WriteInt32LittleEndian(Buffer.Slice(4), Unsafe.As<Index, int>(ref End));

            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(BigInteger Value)
        {
            var Length = Value.GetByteCount();

            Write7BitEncoded(Length);
            Reserve(Length);

            var Span = AsSpan(Position, Length);

            if (Length <= BufferThreshold)
            {
                Span<byte> Buffer = stackalloc byte[Length];

                Value.TryWriteBytes(Buffer, out _);
                Buffer.CopyTo(Span);
            }
            else
            {
                var Buffer = ArrayPool<byte>.Shared.Rent(Length);

                Value.TryWriteBytes(Buffer, out _);
                Buffer.CopyTo(Span);

                ArrayPool<byte>.Shared.Return(Buffer);
            }

            Advance(Length);
        }


        public void Write(RGBColor Value)
        {
            const int Size = 3;
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            MemoryMarshal.Write(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(RGBAColor Value)
        {
            const int Size = 4;
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            MemoryMarshal.Write(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }


        public void Write(Vector2 Value)
        {
            var Size = Unsafe.SizeOf<Geometry.Vector2>();
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            MemoryMarshal.Write(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(Vector2Int Value)
        {
            var Size = Unsafe.SizeOf<Vector2Int>();
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            MemoryMarshal.Write(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

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
            var Size = Unsafe.SizeOf<Vector3>();
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            MemoryMarshal.Write(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        public void Write(Vector3Int Value)
        {
            var Size = Unsafe.SizeOf<Vector3Int>();
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            MemoryMarshal.Write(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

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
            Reserve(5);

            var Index = 0;
            var Span = AsSpan(0, 5);

            while (Value >= 0x80)
            {
                Span[Index++] = (byte)(Value | 0x80);
                Value >>= 7;
            }
            Span[Index++] = (byte)Value;

            Advance(Index);
        }

        public void Write7BitEncoded(ulong Value)
        {
            Reserve(10);

            var Index = 0;
            var Span = AsSpan(0, 10);

            while (Value >= 0x80)
            {
                Span[Index++] = (byte)(Value | 0x80);
                Value >>= 7;
            }
            Span[Index++] = (byte)Value;

            Advance(Index);
        }

        public void Write7BitEncoded(Index Value)
        {
            Write7BitEncoded(Unsafe.As<Index, uint>(ref Value));
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
            Trim(Math.Min(Length, Size));
        }

        public void TrimExcess()
        {
            Trim(Length);
        }


        private void Advance(int Offset)
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

            Span<byte> Buffer = stackalloc byte[Size];
            MemoryMarshal.Write(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }

        private void WriteVector<T>(Vector3<T> Value) where T : unmanaged, INumber<T>
        {
            var Size = Vector3<T>.GetByteCount();
            Reserve(Size);

            Span<byte> Buffer = stackalloc byte[Size];
            MemoryMarshal.Write(Buffer, Value);
            Buffer.CopyTo(AsSpan(Position, Size));

            Advance(Size);
        }
    }
}