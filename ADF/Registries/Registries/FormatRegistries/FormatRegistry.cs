namespace Zion.ADF
{
    internal sealed class FormatRegistry : IWritableRegistry
    {
        private readonly List<DataFormat> Formats;
        private readonly List<FormatCorrection> Corrections;
        private int Writed;

        public bool IsChanged => Writed < Formats.Count;

        public int Count => Formats.Count;


        public FormatRegistry()
        {
            Formats = new(64);
            Corrections = new(16);
        }


        public DataFormat this[uint Id] => Formats[GetIndex(Id)];


        public void Write(ADFObjectWriter Writer)
        {
            Writer.Write("Items", Formats, Writed);
            Writer.Write("Corrections", Corrections);
            Writed = Formats.Count;
            Corrections.Clear();
        }


        public uint Add(DataFormat Format)
        {
            var Id = ADFPrimitives.Count + (uint)Formats.Count;
            Formats.Add(Format);
            return Id;
        }

        public uint AddDeferred(uint Base)
        {
            return Add(DataFormat.GetDeferredFormat(Base));
        }

        public DataFormat Clarify(uint FormatId, ADFRecordObjectWriter Writer)
        {
            return Clarify(FormatId, Writer);
        }

        public DataFormat Clarify(uint FormatId, Parameter[] Parameters)
        {
            var Index = GetIndex(FormatId);
            var Updated = Formats[Index].Clarify(Parameters);

            Formats[Index] = Updated;

            if (Index < Writed)
            {
                Corrections.Add(new(Index, Parameters));
            }

            return Updated;
        }

        public bool IsAssignableFrom(in uint FormatId, in uint TargetFormatId)
        {
            if (FormatId == TargetFormatId || TargetFormatId == ADFPrimitives.Object)
            {
                return true;
            }

            if (IsPrimitive(FormatId))
            {
                return false;
            }

            int BaseFormat = GetIndex(FormatId);
            int Target = GetIndex(TargetFormatId);

            if (BaseFormat < 0 || BaseFormat >= Count || Target < 0 || Target >= Count)
            {
                return false;
            }

            while (true)
            {
                if (BaseFormat == Target)
                {
                    return true;
                }
                if (BaseFormat == 0)
                {
                    return false;
                }

                BaseFormat = (int)Formats[BaseFormat].BaseFormat;
            }
        }

        public bool TryGetFormat(uint FormatId, out DataFormat Format)
        {
            int Index = (int)FormatId - ADFPrimitives.Count;

            if (Index < 0 || Index >= Count)
            {
                Format = default;
                return false;
            }

            Format = Formats[Index];
            return true;
        }


        public IEnumerable<DataFormat> EnumerateHierarchy(DataFormat Low)
        {
            if (!IsRootFormat(Low))
            {
                foreach (var Hierarchy in EnumerateHierarchy(this[Low.BaseFormat]))
                {
                    yield return Hierarchy;
                }
            }

            yield return Low;
        }


        public static bool IsRootFormat(in DataFormat Format)
        {
            return Format.BaseFormat == 0;
        }


        private static bool IsPrimitive(uint Id)
        {
            return Id < ADFPrimitives.Count;
        }

        private static int GetIndex(uint Id)
        {
            if (Id < ADFPrimitives.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(Id), $"Id(={Id}) out of range [{ADFPrimitives.Count}..)");
            }
            return (int)Id - ADFPrimitives.Count;
        }
    }
}