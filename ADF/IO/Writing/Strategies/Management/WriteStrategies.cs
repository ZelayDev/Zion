namespace Zion.ADF
{
    internal sealed class WriteStrategies
    {
        #region Data
        private readonly Dictionary<Type, WriteEntry> Strategies;

        #endregion

        #region Constructors
        public WriteStrategies()
        {
            Strategies = new(256);
            ADFPrimitives.AddTo(Strategies);
        }

        #endregion

        #region PublicMethods
        public WriteEntry<T> GetEntry<T>(ADFWritingContext Context, Type Type)
        {
            ThrowIfNotAssignable<T>(Type);

            if (Strategies.TryGetValue(Type, out var Generalized))
            {
                return (WriteEntry<T>)Generalized;
            }

            var Created = Create<T>(Context, Type);
            Strategies.Add(Type, Created);
            return Created;
        }

        #endregion

        #region Creation
        private WriteEntry<T> Create<T>(ADFWritingContext Context, Type Type)
        {
            if (Type.IsEnum)
            {
                return new
                (
                    Context.Registries.TypeAssociation.GetOrAdd
                    (
                        Type,
                        () => Context.Registries.FormatRegistry.Add(DataFormat.GetEnumFormat<T>())
                    ),
                    new EnumWriteStrategy<T>()
                );
            }

            if (TryCreateLayered<T>(Context, Type, out var LayeredStrategy))
            {
                var LayeredFormatId = GetOrAddDeferred(Context, Type);
                return new
                (
                    LayeredFormatId,
                    LayeredStrategy
                );
            }

            if (ADFSerializers.TryGetSerializer<T>(Type, out var Serializer))
            {
                var SerializableFormatId = GetOrAddDeferred(Context, Type);
                return new
                (
                    SerializableFormatId,
                    ProvidedWriteStrategy<T>.GetStrategy(SerializableFormatId, Serializer)
                );
            }

            if (Type.IsAssignableTo(typeof(IADFWritable)))
            {
                var WritableFormatId = GetOrAddDeferred(Context, Type);
                return new
                (
                    WritableFormatId,
                    ProvidedWriteStrategy<T>.GetStrategy(WritableFormatId)
                );
            }

            var AutoType = AutoSerializationCache.GetOrAdd<T>(Context, Type);
            var FormatId = GetFormatId(Context, AutoType.Schema, Type);
            var Strategy = new AutoWriteStrategy<T>(AutoType.Writer);

            return new(FormatId, Strategy);
        }

        private static bool TryCreateLayered<T>(ADFWritingContext Context, Type Type, out IWriteStrategy<T> Strategy)
        {
            if (!Type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException($"{Type} is not {typeof(T)}");
            }

            if (!DataFormat.HasBase(Type))
            {
                Strategy = default!;
                return false;
            }

            var Layers = new List<ILayerWriteInfo>(10);
            var Kind = WriteKind.None;

            Type? ChildType = null;
            uint ParentFormatId = ADFPrimitives.Object;

            void BuildFormat()
            {
                if (ChildType is not null)
                {
                    ParentFormatId = BuildChildFormat<T>(Context, ChildType, ParentFormatId, Layers[^1]);
                }
            }

            foreach (var LayerType in EnumerateHierarchy(Type))
            {
                if (LayerType.IsDefined(typeof(ADFNonSerializableAttribute), false))
                {
                    continue;
                }

                BuildFormat();
                ChildType = LayerType;

                ILayerWriteInfo LayerInfo;

                if (TryGetSerializer<T>(LayerType, out var Serializer))
                {
                    Kind |= WriteKind.Manual;
                    LayerInfo = Serializer;
                }
                else
                {
                    Kind |= WriteKind.Auto;
                    LayerInfo = AutoSerializationCache.GetOrAdd<T>(Context, LayerType);
                }

                Layers.Add(LayerInfo);
            }

            BuildFormat();

            Strategy = Kind switch
            {
                WriteKind.Auto   => new LayeredAutoWriteStrategy<T>(Layers),
                WriteKind.Manual => new LayeredManualWriteStrategy<T>(Layers),
                WriteKind.Mixed  => new LayeredMixedWriteStrategy<T>(Layers),
                _ => throw new Exception()
            };
            return true;
        }

        private static uint BuildChildFormat<T>(ADFWritingContext Context, Type ChildType, uint BaseFormatId, ILayerWriteInfo LayerInfo)
        {
            var FormatRegistry = Context.Registries.FormatRegistry;
            var TypeAssociation = Context.Registries.TypeAssociation;

            if (TypeAssociation.TryGetFormatId(ChildType, out var Existing))
            {
                return Existing;
            }

            var Format = LayerInfo is IAutoType<T> Auto
                ? DataFormatBuilder.Build(Auto.Schema, BaseFormatId, Context)
                : DataFormatBuilder.BuildDeferred(ChildType, BaseFormatId, FormatRegistry, TypeAssociation);
            
            var FormatId = FormatRegistry.Add(Format);
            TypeAssociation.Add(ChildType, FormatId);

            return FormatId;
        }

        #endregion

        #region PrivateMethods
        private static uint GetOrAddDeferred(ADFWritingContext Context, Type Type)
        {
            return Context.Registries.TypeAssociation.GetOrAddDeferred(Type, Context.Registries.FormatRegistry);
        }

        private static uint GetFormatId(ADFWritingContext Context, TypeSchema Schema, Type Type)
        {
            var TypeAssociation = Context.Registries.TypeAssociation;

            uint Create()
            {
                var Format = DataFormatBuilder.Build(Schema, Context);
                return Context.Registries.FormatRegistry.Add(Format);
            }

            return TypeAssociation.GetOrAdd(Type, Create);
        }


        private static IEnumerable<Type> EnumerateHierarchy(Type? Type)
        {
            while (DataFormat.HasBase(Type))
            {
                yield return Type;
                Type = Type.BaseType;
            }
        }

        private static bool TryGetSerializer<T>(Type Layer, out IADFSerializer<T> Serializer)
        {
            return ADFSerializers.TryGetSerializer(Layer, out Serializer)
                || WritableLayerWriterCache<T>.TryGetSerializer(Layer, out Serializer);
        }


        private static void ThrowIfNotAssignable<T>(Type Type)
        {
            if (!Type.IsAssignableTo(typeof(T)))
            {
                throw new InvalidCastException($"{Type} is not assignable from {typeof(T)}");
            }
        }

        #endregion
    }
}