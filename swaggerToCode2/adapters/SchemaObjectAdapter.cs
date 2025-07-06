using OpenApi.Models;
using swaggerToCode2.providers;
using System.Linq;
using System.Xml.Schema;
using SwaggerToCode.Models;

namespace SwaggerToCode.Adapters
{
    public class ErrorSchemaObject : ISchemaObject
    {
        // Store the reference that couldn't be found
        private readonly string _ref;

        public ErrorSchemaObject(string refValue)
        {
            _ref = refValue;
        }

        // Only property that won't throw an exception
        public string? Ref
        {
            get => _ref;
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        // All other properties throw exceptions
        public string? Type
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public SchemaObjectType ObjectType
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public string? Format
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public Dictionary<string, ISchemaObject>? Properties
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public ISchemaObject? Items
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public List<string>? Required
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public List<string>? Enum
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public List<ISchemaObject>? AllOf
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public List<ISchemaObject>? OneOf
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public List<ISchemaObject>? AnyOf
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public string? Description
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public object? Default
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public object? AdditionalProperties
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public IDiscriminatorObject? Discriminator
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public Dictionary<string, object> VendorExtensions
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public object? Example
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public Dictionary<string, IExampleObject>? Examples
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public object? this[string key]
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }

        public string? Name
        {
            get => throw new NotImplementedException($"Reference schema can't be found {_ref}");
            set => throw new NotImplementedException($"Reference schema can't be found {_ref}");
        }
    }

    public class CompositeSchemaObject : ISchemaObject
    {
        private List<ISchemaObject> _schemaObjects;
        private string _name;

        public CompositeSchemaObject(string name, ISchemaObject schemaObject)
        {
            _name = name;
            _schemaObjects = new List<ISchemaObject>() { schemaObject };
        }

        public void AddSchemaObject(ISchemaObject schemaObject)
        {
            _schemaObjects.Add(schemaObject);
        }

        public List<ISchemaObject> Schemas => _schemaObjects;

        public string? Ref
        {
            get => "";
            set => throw new NotImplementedException("Setting Ref on this is not supported");
        }

        public string? Type
        {
            get => "object";
            set => throw new NotImplementedException("Setting Type on this is not supported");
        }

        public SchemaObjectType ObjectType
        {
            get => SchemaObjectType.Object;
            set => throw new NotImplementedException("Setting this Type on is not supported");
        }

        public string? Format
        {
            get => _schemaObjects.FirstOrDefault()?.Format ?? "";
            set => throw new NotImplementedException("Setting Format is not supported");
        }

        public Dictionary<string, ISchemaObject>? Properties
        {
            get => _schemaObjects
                .Where(schemaObject => schemaObject.Properties != null)
                .SelectMany(schemaObject => schemaObject.Properties)
                .ToDictionary();
            set => throw new NotImplementedException("Setting Properties is not supported");
        }

        public ISchemaObject? Items
        {
            get => null;
            set => throw new NotImplementedException("Setting Items is not supported");
        }

        public List<string>? Required
        {
            get => _schemaObjects
                .Where(schemaObject => schemaObject.Required != null)
                .SelectMany(schemaObject => schemaObject.Required)
                .ToList();
            set => throw new NotImplementedException("Setting Required is not supported");
        }

        public List<string>? Enum
        {
            get => new List<string>();
            set => throw new NotImplementedException("Setting Enum is not supported");
        }

        public List<ISchemaObject>? AllOf
        {
            get => null;
            set => throw new NotImplementedException("Setting AllOf is not supported");
        }

        public List<ISchemaObject>? OneOf
        {
            get => null;
            set => throw new NotImplementedException("Setting OneOf is not supported");
        }

        public List<ISchemaObject>? AnyOf
        {
            get => null;
            set => throw new NotImplementedException("Setting AnyOf is not supported");
        }

        public string? Description
        {
            get => _schemaObjects.FirstOrDefault()?.Description ?? "";
            set => throw new NotImplementedException("Setting Description is not supported");
        }

        public object? Default
        {
            get => _schemaObjects.FirstOrDefault()?.Default ?? null;
            set => throw new NotImplementedException("Setting Default is not supported");
        }

        public object? AdditionalProperties
        {
            get => _schemaObjects
                .Where(schemaObject => schemaObject.AdditionalProperties != null)
                .Select(schemaObject => schemaObject.AdditionalProperties)
                .ToList();
            set => throw new NotImplementedException("Setting AdditionalProperties is not supported");
        }

        public IDiscriminatorObject? Discriminator
        {
            get => null;
            set => throw new NotImplementedException("Setting Discriminator is not supported");
        }

        public Dictionary<string, object> VendorExtensions
        {
            get => _schemaObjects.Where(schemaObject => schemaObject.VendorExtensions != null)
                .SelectMany(schemaObject => schemaObject.VendorExtensions).ToDictionary();
        }

        public object? Example
        {
            get => null;
            set => throw new NotImplementedException("Setting Example is not supported");
        }

        public Dictionary<string, IExampleObject>? Examples
        {
            get => null;
            set => throw new NotImplementedException("Setting Examples is not supported");
        }

        public object? this[string key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public string? Name
        {
            get => _name;
            set => _name = value;
        }
    }

    public interface ISchemaObjectAdapter : ISchemaObject
    {
        TypeAdapter TypeAdapter { get; }
    }

public class SchemaObjectAdapter : ISchemaObjectAdapter, TypeAdapter
    {
        private ISchemaObject _schema;
        private List<ISchemaObjectField>? _fields;
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
            if (!string.IsNullOrEmpty(_schema.Ref))
            {
                _schema = adapterProvider.FindSchemaByReference(_schema.Ref) ?? schema;
            }
            else if (_schema.AllOf != null)
            {
                ISchemaObject schemaFirst = schema.AllOf!
                    .Select
                    (
                        schemaObject => 
                        string.IsNullOrEmpty(schemaObject.Ref) ? 
                            schemaObject : 
                            adapterProvider.FindSchemaByReference(schemaObject.Ref) ?? new ErrorSchemaObject(schemaObject.Ref)
                    )
                    .First();
                if (!(schemaFirst is ErrorSchemaObject))
                {
                    _schema = new CompositeSchemaObject(schema.Name, schemaFirst);
                    
                    schema.AllOf!.Skip(1)
                        .Select
                        (
                            schemaObject => 
                                string.IsNullOrEmpty(schemaObject.Ref) ? 
                                    schemaObject : 
                                    adapterProvider.FindSchemaByReference(schemaObject.Ref) ?? new ErrorSchemaObject(schemaObject.Ref)
                        )
                        .ToList()
                        .ForEach(schemaObject => (_schema as CompositeSchemaObject)!.AddSchemaObject(schemaObject));
                }

                _fields = _schema.Properties?
                    .Select
                    (
                        propPair =>
                        {
                            return adapterProvider.CreateSchemaObjectFieldAdapter
                                (
                                    propPair.Key, 
                                    propPair.Value
                                );
                        }
                    ).ToList() ?? [];
            }
            _adapterProvider = adapterProvider;
            _typeAdapt = typeAdaptProvider.GetTypeAdapter(templateConfig, Name, schema);
        }

        public TypeAdapter TypeAdapter => _typeAdapt;
        
        public List<ISchemaObject> Schemas => _schema is CompositeSchemaObject schemaObject ? 
            schemaObject.Schemas.Select(schema => _adapterProvider.CreateSchemaObjectAdapter("", schema)).ToList()
            : new List<ISchemaObject>(){this};

        public string? Ref
        {
            get => "";
            set => throw new NotImplementedException("Setting Ref on this is not supported");
        }

        public string? Type
        {
            get => _schema.Type;
            set => throw new NotImplementedException("Setting Type on this is not supported");
        }

        public SchemaObjectType ObjectType
        {
            get => _schema.ObjectType;
            set => throw new NotImplementedException("Setting ObjectType on this is not supported");
        }

        public string? Format
        {
            get => _schema.Format;
            set => throw new NotImplementedException("Setting Format on this is not supported");
        }

        public Dictionary<string, ISchemaObject>? Properties
        {
            get => _schema.Properties?.ToDictionary(
                kvp => kvp.Key,
                kvp => _adapterProvider.CreateSchemaObjectAdapter(kvp.Key, kvp.Value)) ?? null;
            set => _schema.Properties = value;
        }

        public ISchemaObject? Items
        {
            get => _schema.Items != null
                ? _adapterProvider.CreateSchemaObjectAdapter("", _schema.Items)
                : null;
            set => _schema.Items = value;
        }

        public List<string>? Required
        {
            get => _schema.Required;
            set => _schema.Required = value;
        }

        public List<string>? Enum
        {
            get => _schema.Enum;
            set => _schema.Enum = value;
        }

        public List<ISchemaObject>? AllOf
        {
            get => _schema.AllOf?.Select(schema =>
                _adapterProvider.CreateSchemaObjectAdapter("", schema)).ToList() ?? null;
            set => _schema.AllOf = value;
        }

        public List<ISchemaObject>? OneOf
        {
            get => _schema.OneOf?.Select(schema =>
                _adapterProvider.CreateSchemaObjectAdapter("", schema)).ToList() ?? null;
            set => _schema.OneOf = value;
        }

        public List<ISchemaObject>? AnyOf
        {
            get => _schema.AnyOf?.Select(schema =>
                _adapterProvider.CreateSchemaObjectAdapter("", schema)).ToList() ?? null;
            set => _schema.AnyOf = value;
        }

        public string? Description
        {
            get => _schema.Description;
            set => _schema.Description = value;
        }

        public object? Default
        {
            get => _schema.Default;
            set => _schema.Default = value;
        }

        public object? AdditionalProperties
        {
            get => _schema.AdditionalProperties;
            set => _schema.AdditionalProperties = value;
        }

        public IDiscriminatorObject? Discriminator
        {
            get => _schema.Discriminator != null
                ? _adapterProvider.CreateDiscriminatorObjectAdapter(_schema.Discriminator) as DiscriminatorObject
                : null;
            set => _schema.Discriminator = value;
        }

        public Dictionary<string, object> VendorExtensions => _schema.VendorExtensions;
        public object? Example { get => _schema.Example; set => _schema.Example = value; }
        public Dictionary<string, IExampleObject>? Examples { get => _schema.Examples; set => _schema.Examples = value; }

        public object? this[string key]
        {
            get => _schema[key];
            set => _schema[key] = value;
        }

        public string? Name
        {
            get => _schema.Name;
            set => _schema.Name = value;
        }

        public string OutputName => TypeAdapter.OutputName;
        public string TypeName  => TypeAdapter.TypeName;
        public string ListTypeName => TypeAdapter.ListTypeName;
        public string MemberPrefix => TypeAdapter.MemberPrefix;
        public bool IsSimpleType => TypeAdapter.IsSimpleType;

        public bool IsObjectType => TypeAdapter.IsObjectType;
        public bool IsArrayType => TypeAdapter.IsArrayType;
        public bool IsCompositeType => TypeAdapter.IsCompositeType;
        public bool IsEnumType => TypeAdapter.IsEnumType;
        public List<ISchemaObjectField> Fields => _fields ?? [];
        public List<ISchemaObject> References { get; }

    }

    public class SchemaObjectFieldAdapter : ISchemaObjectField, TypeAdapter
    {
        private readonly string _name;
        private readonly ISchemaObject _property;
        private readonly TypeAdapter _typeAdapt;

        public SchemaObjectFieldAdapter
        (
            string name,
            ISchemaObject property,
            AdapterProvider adapterProvider,
            TypeAdapterProvider typeAdapterProvider,
            TemplateConfig templateConfig
        )
        {
            _name = name;
            if (!string.IsNullOrEmpty(property.Ref))
            {
                _property = adapterProvider.FindSchemaByReference(property.Ref) ??
                            new ErrorSchemaObject(property.Ref);
            }
            else if (property.OneOf != null && property.ObjectType == SchemaObjectType.String)
            {
                // This property is an enum, should assert the schema ref is an enum...
                if (!string.IsNullOrEmpty(property.Ref))
                {
                    _property = adapterProvider.FindSchemaByReference(property.Ref) ??
                                new ErrorSchemaObject(property.Ref);
                    if(_property.Enum?.Count > 0)
                    {
                        _property = new ErrorSchemaObject(property.Ref);
                    }
                }
            }

            _property = adapterProvider.CreateSchemaObjectAdapter("", property);
            _typeAdapt = typeAdapterProvider.GetTypeAdapter(templateConfig, Name, _property);
        }

        public string Name => _name;

        public ISchemaObject Field => _property;

        public string OutputName => _typeAdapt.OutputName;
        public string TypeName => _typeAdapt.TypeName;
        public string ListTypeName => _typeAdapt.ListTypeName;
        public string MemberPrefix => _typeAdapt.MemberPrefix;
        public bool IsSimpleType => _typeAdapt.IsSimpleType;
        public bool IsObjectType => _typeAdapt.IsObjectType;
        public bool IsArrayType => _typeAdapt.IsArrayType;
        public bool IsCompositeType => _typeAdapt.IsCompositeType;
        public bool IsEnumType => _typeAdapt.IsEnumType;
    }
}