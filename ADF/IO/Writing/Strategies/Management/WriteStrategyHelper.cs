namespace Zion.ADF
{
    internal static class WriteStrategyHelper<T>
    {
        public static bool Setup(ADFWritingContext Context, T Value, ref StreamGroup Target)
        {
            if (WriteStrategyHelper<T>.ResolveWriteTarget(Context, Value, ref Target))
            {
                return true;
            }

            WriteStrategyHelper<T>.ClarifyIfNotSealed(Context, Target.BaseStream, Value!.GetType());
            return false;
        }


        public static void ClarifyIfNotSealed(ADFWritingContext Context, ArenaStream Target, Type ObjectType)
        {
            if (!typeof(T).IsSealed)
            {
                Target.WriteCompressed
                (
                    Context,
                    Context.Registries.TypeAssociation.GetOrAddDeferred(ObjectType, Context.Registries.FormatRegistry)
                );
            }
        }

        public static bool ResolveWriteTarget(ADFWritingContext Context, T Value, ref StreamGroup Base)
        {
            if (Value is null)
            {
                Base.BaseStream.WriteCompressedZero(Context);
                return true;
            }

            if (Value.GetType().IsValueType)
            {
                return false;
            }

            if(Context.Registries.References.TryGetReference(Value, out var Existing))
            {
                Base.BaseStream.WriteCompressed(Context, Existing.Id);
                return true;
            }

            var Link = Reference.CreateNewReference(Base.ChildsLength);
            Base.BaseStream.WriteCompressed(Context, Link);

            var Target = new StreamGroup(Context.Arena.GetStream(1));
            Base.Add(Target);
            Base = Target;

            return false;
        }
    }
}