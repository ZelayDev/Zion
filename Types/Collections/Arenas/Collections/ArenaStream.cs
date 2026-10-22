using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Zion.Vectors;
using static System.Buffers.Binary.BinaryPrimitives;
using Vector2 = Zion.Vectors.Vector2;
using Vector3 = Zion.Vectors.Vector3;

namespace Zion
{
    public sealed class ArenaStream : ArenaCollection<byte>
    {
        public new int Length
        {
            get;
            private set
            {
                Modify();
                field = value;
            }
        }

        private int _Position;
        public int Position
        {
            get => _Position;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                Modify();
                _Position = value;
            }
        }


        public ArenaStream(ArenaSpan<byte> Data) : base(Data) { }


        public new byte this[int Index]
        {
            get => base[Index];
            set => base[Index] = value;
        }

        public new byte this[Index Index]
        {
            get => base[Index];
            set => base[Index] = value;
        }


        protected override int GetSpanLimit() => -1;

        protected override IEnumerator<int> GetIndexEnumerator()
        {
            int Count = Length;
            for (int i = 0; i < Count; i++)
            {
                yield return i;
            }
        }


        public void Write(bool Value)
        {
            Write(Value ? (byte)1 : (byte)0);
        }

        public void Write(byte Value)
        {
            Reserve(1);
            this[Position] = Value;
            UpdateLengthFromPosition(Position + 1);
        }

        public void Write(sbyte Value)
        {
            Write((byte)Value);
        }

        public void Write(char Value)
        {
            Write((ushort)Value);
        }

        public void Write(decimal Value)
        {
            Reserve(sizeof(decimal));
            UseSpan
            (
                Span =>
                {
                    var Destination = MemoryMarshal.Cast<byte, int>(Span);
                    decimal.GetBits(Value, Destination);
                    UpdateLengthFromPosition(_Position + sizeof(decimal));
                }
            );
        }

        public void Write(double Value)
        {
            Write(Value, sizeof(double), WriteDoubleLittleEndian);
        }

        public void Write(float Value)
        {
            Write(Value, sizeof(float), WriteSingleLittleEndian);
        }

        public void Write(int Value)
        {
            Write(Value, sizeof(int), WriteInt32LittleEndian);
        }

        public void Write(uint Value)
        {
            Write(Value, sizeof(uint), WriteUInt32LittleEndian);
        }

        public void Write(long Value)
        {
            Write(Value, sizeof(long), WriteInt64LittleEndian);
        }

        public void Write(ulong Value)
        {
            Write(Value, sizeof(ulong), WriteUInt64LittleEndian);
        }

        public void Write(short Value)
        {
            Write(Value, sizeof(short), WriteInt16LittleEndian);
        }

        public void Write(ushort Value)
        {
            Write(Value, sizeof(ushort), WriteUInt16LittleEndian);
        }

        public void Write(string Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

            int Length = Encoding.UTF8.GetByteCount(Value);
            
            Write7BitEncodedInt(Length);
            Reserve(Length);

            UseSpan
            (
                Span => Encoding.UTF8.GetBytes(Value, Span)
            );

            UpdateLengthFromPosition(Position + Length);
        }


        public void Write(Half Value)
        {
            Write(Value, 2, WriteHalfLittleEndian);
        }

        public void Write(Index Value)
        {
            Reserve(4);
            Write(Unsafe.As<Index, uint>(ref Value));
        }

        public void Write(Range Value)
        {
            Reserve(2 * (sizeof(bool) + sizeof(int)));
            WriteIndex(Value.Start);
            WriteIndex(Value.End);
        }

        public void Write(BigInteger Value)
        {
            int Position = _Position;
            int Length = Value.GetByteCount();

            Reserve(Length + 4);
            UseSpan
            (
                Span =>
                {
                    WriteInt32LittleEndian(Span, Length);
                    Value.TryWriteBytes(Span.Slice(4), out _);   
                }
            );
            UpdateLengthFromPosition(Position + Length + 4);
        }


        public void Write(RGBColor Value)
        {
            Reserve(3);
            UseSpan
            (
                Span =>
                {
                    Span[0] = Value.R;
                    Span[1] = Value.G;
                    Span[2] = Value.B;
                }
            );
            UpdateLengthFromPosition(_Position + 3);
        }

        public void Write(RGBAColor Value)
        {
            Reserve(4);
            UseSpan
            (
                Span =>
                {
                    Span[0] = Value.R;
                    Span[1] = Value.G;
                    Span[2] = Value.B;
                    Span[3] = Value.A;
                }
            );
            UpdateLengthFromPosition(_Position + 4);
        }


        public void Write(Vector2 Value)
        {
            Reserve(8);
            UseSpan
            (
                Span =>
                {
                    WriteSingleLittleEndian(Span, Value.X);
                    WriteSingleLittleEndian(Span.Slice(4), Value.Y);
                }
            );
            UpdateLengthFromPosition(_Position + 8);
        }

        public void Write(Vector2Int Value)
        {
            Reserve(8);
            UseSpan
            (
                Span =>
                {
                    WriteInt32LittleEndian(Span, Value.X);
                    WriteInt32LittleEndian(Span.Slice(4), Value.Y);
                }
            );
            UpdateLengthFromPosition(_Position + 8);
        }

        public void Write(Vector3 Value)
        {
            Reserve(12);
            UseSpan
            (
                Span =>
                {
                    WriteSingleLittleEndian(Span, Value.X);
                    WriteSingleLittleEndian(Span.Slice(4), Value.Y);
                    WriteSingleLittleEndian(Span.Slice(8), Value.Z);
                }
            );
            UpdateLengthFromPosition(_Position + 12);
        }

        public void Write(Vector3Int Value)
        {
            Reserve(12);
            UseSpan
            (
                Span =>
                {
                    WriteInt32LittleEndian(Span, Value.X);
                    WriteInt32LittleEndian(Span.Slice(4), Value.Y);
                    WriteInt32LittleEndian(Span.Slice(8), Value.Z);
                }
            );
            UpdateLengthFromPosition(_Position + 12);
        }

        public void Write(System.Numerics.Vector2 Value)
        {
            Reserve(8);
            UseSpan
            (
                Span =>
                {
                    WriteSingleLittleEndian(Span, Value.X);
                    WriteSingleLittleEndian(Span.Slice(4), Value.Y);
                }
            );
            UpdateLengthFromPosition(_Position + 8);
        }

        public void Write(System.Numerics.Vector3 Value)
        {
            Reserve(8);
            UseSpan
            (
                Span =>
                {
                    WriteSingleLittleEndian(Span, Value.X);
                    WriteSingleLittleEndian(Span.Slice(4), Value.Y);
                }
            );
            UpdateLengthFromPosition(_Position + 8);
        }


        public void Write(ArenaStream Value)
        {
            int Length = Value.Length;
            Reserve(Length);

            UseSpan( Value.CopyTo);
            
            UpdateLengthFromPosition(_Position + Length);
        }


        public void Write7BitEncodedInt(int Value)
        {
            Write7BitEncodedUInt((uint)((Value << 1) ^ (Value >> 31)));
        }

        public void Write7BitEncodedInt64(long Value)
        {
            Write7BitEncodedUInt64((ulong)((Value << 1) ^ (Value >> 63)));
        }

        public void Write7BitEncodedUInt(uint Value)
        {
            Reserve(5);
            var Index = 0;

            UseSpan
            (
                Span =>
                {
                    while (Value >= 0x80)
                    {
                        Span[Index++] = (byte)(Value | 0x80);
                        Value >>= 7;
                    }
                    Span[Index++] = (byte)Value;
                }
            );

            UpdateLengthFromPosition(_Position + Index);
        }

        public void Write7BitEncodedUInt64(ulong Value)
        {
            Reserve(10);
            var Index = 0;

            UseSpan
            (
                Span =>
                {
                    while (Value >= 0x80)
                    {
                        Span[Index++] = (byte)(Value | 0x80);
                        Value >>= 7;
                    }
                    Span[Index++] = (byte)Value;
                }
            );

            UpdateLengthFromPosition(_Position + Index);
        }

        public void Write7BitEncodedIndex(Index Value)
        {
            Write7BitEncodedUInt(Unsafe.As<Index, uint>(ref Value));
        }


        private void Write<T>(T Value, int Size, Action<Span<byte>, T> Write)
        {
            Reserve(Size);
            UseSpan
            (
                Span =>
                {
                    Write(Span, Value);
                }
            );
            UpdateLengthFromPosition(_Position + Size);
        }

        private void WriteIndex(Index Value)
        {
            this[_Position++] = Value.IsFromEnd ? (byte)1 : (byte)0;
            Write(Value.Value, sizeof(int), WriteInt32LittleEndian);
        }


        public byte[] ToArray()
        {
            return ToArray(0, Length);
        }


        public new void UseSpan(Action<Span<byte>> Action)
        {
            base.UseSpan(Action);
        }

        public new void UseSpan(int Count, Action<Span<byte>> Action)
        {
            base.UseSpan(_Position, Count, Action);
            UpdateLengthFromPosition(Count);
        }

        public new void UseSpan(int Start, int Count, Action<Span<byte>> Action)
        {
            base.UseSpan(Start, Count, Action);
            UpdateLengthFromPosition(Start + Count);
        }


        public new void UseSpan<I>(I Other, Action<Span<byte>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Other, Action);
        }

        public new void UseSpan<I>(int Count, I Other, Action<Span<byte>, I> Action) where I : allows ref struct
        {
            base.UseSpan(_Position, Count, Other, Action);
            UpdateLengthFromPosition(Count);
        }

        public new void UseSpan<I>(int Start, int Count, I Other, Action<Span<byte>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Start, Count, Other, Action);
            UpdateLengthFromPosition(Start + Count);
        }


        public new R UseSpan<R>(Func<Span<byte>, R> Function)
        {
            return base.UseSpan(Function);
        }

        public new R UseSpan<R>(int Count, Func<Span<byte>, R> Function)
        {
            var Result = base.UseSpan(_Position, Count, Function);
            UpdateLengthFromPosition(Count);
            return Result;
        }

        public new R UseSpan<R>(int Start, int Count, Func<Span<byte>, R> Function)
        {
            var Result = base.UseSpan(Start, Count, Function);
            UpdateLengthFromPosition(Start + Count);
            return Result;
        }


        public new R UseSpan<I, R>(I Other, Func<Span<byte>, I, R> Action) where I : allows ref struct
        {
            return base.UseSpan(Other, Action);
        }

        public new R UseSpan<I, R>(int Count, I Other, Func<Span<byte>, I, R> Action) where I : allows ref struct
        {
            var Result = base.UseSpan(_Position, Count, Other, Action);
            UpdateLengthFromPosition(Count);
            return Result;
        }

        public new R UseSpan<I, R>(int Start, int Count, I Other, Func<Span<byte>, I, R> Action) where I : allows ref struct
        {
            var Result = base.UseSpan(Start, Count, Other, Action);
            UpdateLengthFromPosition(Start + Count);
            return Result;
        }


        public new void UseReadOnlySpan(Action<ReadOnlySpan<byte>> Action)
        {
            base.UseReadOnlySpan(Action);
        }

        public new void UseReadOnlySpan(int Count, Action<ReadOnlySpan<byte>> Action)
        {
            base.UseReadOnlySpan(_Position, Count, Action);
        }

        public new void UseReadOnlySpan(int Start, int Count, Action<ReadOnlySpan<byte>> Action)
        {
            base.UseReadOnlySpan(Start, Count, Action);
        }


        public new void UseReadOnlySpan<I>(I Other, Action<ReadOnlySpan<byte>, I> Action) where I : allows ref struct
        {
            base.UseReadOnlySpan(Other, Action);
        }

        public new void UseReadOnlySpan<I>(int Count, I Other, Action<ReadOnlySpan<byte>, I> Action) where I : allows ref struct
        {
            base.UseReadOnlySpan(_Position, Count, Other, Action);
        }

        public new void UseReadOnlySpan<I>(int Start, int Count, I Other, Action<ReadOnlySpan<byte>, I> Action) where I : allows ref struct
        {
            base.UseReadOnlySpan(Start, Count, Other, Action);
        }


        public new R UseReadOnlySpan<R>(Func<ReadOnlySpan<byte>, R> Function)
        {
            return base.UseReadOnlySpan(Function);
        }

        public new R UseReadOnlySpan<R>(int Count, Func<ReadOnlySpan<byte>, R> Function)
        {
            return base.UseReadOnlySpan(_Position, Count, Function);
        }

        public new R UseReadOnlySpan<R>(int Start, int Count, Func<ReadOnlySpan<byte>, R> Function)
        {
            return base.UseReadOnlySpan(Start, Count, Function);
        }


        public new R UseReadOnlySpan<I, R>(I Other, Func<ReadOnlySpan<byte>, I, R> Action) where I : allows ref struct
        {
            return base.UseReadOnlySpan(Other, Action);
        }

        public new R UseReadOnlySpan<I, R>(int Count, I Other, Func<ReadOnlySpan<byte>, I, R> Action) where I : allows ref struct
        {
            return base.UseReadOnlySpan(_Position, Count, Other, Action);
        }

        public new R UseReadOnlySpan<I, R>(int Start, int Count, I Other, Func<ReadOnlySpan<byte>, I, R> Action) where I : allows ref struct
        {
            return base.UseReadOnlySpan(Start, Count, Other, Action);
        }


        public new void CopyTo(Span<byte> Destination)
        {
            CopyTo(0, Length, Destination);
        }

        public void CopyTo(Stream Stream)
        {
            ArgumentNullException.ThrowIfNull(Stream);
            UseReadOnlySpan(Span => Stream.Write(Span.Slice(0, Length)));
        }


        public void Reserve(int Size)
        {
            int Required = _Position + Size;

            if (Required > base.Length)
            {
                int NewSize = base.Length < 256
                    ? Math.Max(base.Length * 2, Required)
                    : Math.Max(base.Length + 256, Required);

                Expand(NewSize);
            }
        }

        public new void TrimExcess(int Size)
        {
            base.TrimExcess(Size);
        }

        public void TrimExcess()
        {
            TrimExcess(Length);
        }


        private void UpdateLengthFromPosition(int Position)
        {
            _Position = Position;
            if (Position > Length)
            {
                Length = Position;
            }
        }
    }
}