using OpenApi.Models;
using swaggerToCode2.providers;
using System.Linq;
using SwaggerToCode.Models;

namespace SwaggerToCode.Adapters
{
    public class SchemaObjectAdapter : ISchemaObject, TypeAdapter
    {
        private readonly ISchemaObject _schema;
        private readonly AdapterProvider _adapterProvider;
        private readonly TypeAdapter _typeAdapt;

        public SchemaObjectAdapter
        (
            ISchemaObject schema, 
            AdapterProvider adapterProvider, 
            TypeAdapterProvider typeAdaptProvider, 
            TemplateConfig templateConfig
        )
        {
            _schema = schema;
            _adapterProvider = adapterProvider;
            _typeAdapt = typeAdaptProvider.GetTypeAdapter(templateConfig, Name, schema);
        }

        public TypeAdapter TypeAdapter => _typeAdapt;
        public string Ref
        {
            get => _schema.Ref;
            set => _schema.Ref = value;
        }

        public string Type
        {
            get => _schema.Type;
            set => _schema.Type = value;
        }

        public string Format
        {
            get => _schema.Format;
            set => _schema.Format = value;
        }

        public Dictionary<string, ISchemaObject>? Properties
        {
            get => _schema.Properties?.ToDictionary(
                kvp => kvp.Key,
                kvp => _adapterProvider.CreateSchemaObjectAdapter(kvp.Value)) ?? null;
            set => _schema.Properties = value;
        }

        public ISchemaObject? Items
        {
            get => _schema.Items != null
                ? _adapterProvider.CreateSchemaObjectAdapter(_schema.Items) as SchemaObject
                : null;
            set => _schema.Items = value;
        }

        public List<string> Required
        {
            get => _schema.Required;
            set => _schema.Required = value;
        }

        public List<string> Enum
        {
            get => _schema.Enum;
            set => _schema.Enum = value;
        }

        public List<ISchemaObject>? AllOf
        {
            get => _schema.AllOf?.Select(schema =>
                _adapterProvider.CreateSchemaObjectAdapter(schema)).ToList() ?? null;
            set => _schema.AllOf = value;
        }

        public List<ISchemaObject>? OneOf
        {
            get => _schema.OneOf?.Select(schema =>
                _adapterProvider.CreateSchemaObjectAdapter(schema)).ToList() ?? null;
            set => _schema.OneOf = value;
        }

        public List<ISchemaObject>? AnyOf
        {
            get => _schema.AnyOf?.Select(schema =>
                _adapterProvider.CreateSchemaObjectAdapter(schema)).ToList() ?? null;
            set => _schema.AnyOf = value;
        }

        public string Description
        {
            get => _schema.Description;
            set => _schema.Description = value;
        }

        public object Default
        {
            get => _schema.Default;
            set => _schema.Default = value;
        }

        public object AdditionalProperties
        {
            get => _schema.AdditionalProperties;
            set => _schema.AdditionalProperties = value;
        }

        public DiscriminatorObject Discriminator
        {
            get => _schema.Discriminator != null
                ? _adapterProvider.CreateDiscriminatorObjectAdapter(_schema.Discriminator) as DiscriminatorObject
                : null;
            set => _schema.Discriminator = value;
        }

        public Dictionary<string, object> VendorExtensions => _schema.VendorExtensions;
        public object? Example { get => _schema.Example; set => _schema.Example = value; }
        public Dictionary<string, IExampleObject>? Examples { get => _schema.Examples; set => _schema.Examples = value; }

        public object this[string key]
        {
            get => _schema[key];
            set => _schema[key] = value;
        }

        public string Name
        {
            get => _schema.Name;
            set => _schema.Name = value;
        }

        public string OutputName => _typeAdapt.OutputName;
        public string TypeName  => _typeAdapt.TypeName;
        public string ListTypeName => _typeAdapt.ListTypeName;
        public string MemberPrefix => _typeAdapt.MemberPrefix;

        public bool IsReference => _schema.IsReference;

        public bool IsSimpleType => _schema.IsSimpleType;

        public bool IsArrayType => _schema.IsArrayType;
        public bool IsObjectType => _schema.IsObjectType;

        public ISchemaObject? ReferenceSchemaObject
        {
            get => _schema.ReferenceSchemaObject != null
                ? _adapterProvider.CreateSchemaObjectAdapter(_schema.ReferenceSchemaObject) as SchemaObject
                : null;
            set => _schema.ReferenceSchemaObject = value;
        }

        public List<ISchemaObjectField> Fields => _schema.Fields.Select(field =>
            _adapterProvider.CreateSchemaObjectFieldAdapter(field) as ISchemaObjectField).ToList();

        public bool UpdateSchemaReferences(ISchemaObjectFinder finder)
        {
            return false;
        }

        public List<ISchemaObject> References { get; }
    }

    public class SchemaObjectFieldAdapter : ISchemaObjectField, TypeAdapter
    {
        private readonly ISchemaObjectField _field;
        private SchemaObjectAdapter _fieldAdapter;
        private readonly AdapterProvider _adapterProvider;
        private readonly TypeAdapter _typeAdapt;

        public SchemaObjectFieldAdapter
        (
            ISchemaObjectField field,
            AdapterProvider adapterProvider,
            TypeAdapterProvider typeAdaptProvider,
            TemplateConfig templateConfig
        )
        {
            _field = field;
            _adapterProvider = adapterProvider;
            _fieldAdapter = _adapterProvider.CreateSchemaObjectAdapter(_field.Field) as SchemaObjectAdapter;
            _typeAdapt = typeAdaptProvider.GetTypeAdapter(templateConfig, Name, _fieldAdapter);
        }

        public ISchemaObject Parent => _field.Parent != null
            ? _adapterProvider.CreateSchemaObjectAdapter(_field.Parent) as SchemaObject
            : null;

        public string Name => _field.Name;

        public ISchemaObject Field
        {
            get
            {
                return _fieldAdapter;
            }
        }

        public string OutputName => _typeAdapt.OutputName;
        public string TypeName => _typeAdapt.TypeName;
        public string ListTypeName => _typeAdapt.ListTypeName;
        public string MemberPrefix => _typeAdapt.MemberPrefix;
    }
}