using System.Diagnostics.CodeAnalysis;

namespace Zion.ADF
{
    internal readonly struct Parameter
    {
        public readonly uint NameId;
        public readonly uint FormatId;


        public Parameter(uint NameId, uint FormatId)
        {
            if (NameId == 0u)
            {
                throw new ArgumentNullException(nameof(NameId), "Null StringId");
            }

            this.NameId = NameId;
            this.FormatId = FormatId;
        }


        public static bool operator ==(Parameter A, Parameter B)
        {
            return A.NameId == B.NameId && A.FormatId == B.FormatId;
        }

        public static bool operator !=(Parameter A, Parameter B)
        {
            return A.NameId != B.NameId || A.FormatId != B.FormatId;
        }


        public override string ToString()
        {
            return $"[{NameId}; {FormatId}]";
        }

        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is Parameter Parameter && this == Parameter;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NameId, FormatId);
        }
    }
}