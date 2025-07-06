using swagger.utils;
using YamlDotNet.Serialization;

namespace OpenApi.Models
{

    public class SchemaObject : ISchemaObject
    {
        private readonly Dictionary<string, object> _vendorExtensions = new();

        [YamlMember(Alias = "$ref")]
        public string? Ref { get; set; }

        [YamlMember(Alias = "type")]
        public string? Type { get; set; }

        [YamlIgnore]
        public SchemaObjectType ObjectType
        {
            get => EnumExtensions.GetEnumFromStringValue<SchemaObjectType>(Type ?? "other");
            set => Type = value.GetStringValue();
        }

        [YamlMember(Alias = "format")]
        public string? Format { get; set; }

        [YamlMember(Alias = "properties")]
        public Dictionary<string, ISchemaObject>? Properties { get; set; }

        [YamlMember(Alias = "items")]
        public ISchemaObject? Items { get; set; }

        [YamlMember(Alias = "required")]
        public List<string>? Required { get; set; }

        [YamlMember(Alias = "enum")]
        public List<string>? Enum { get; set; }

        [YamlMember(Alias = "allOf")]
        public List<ISchemaObject>? AllOf { get; set; }

        // If OneOf is not null or empty, Discriminator must not be null
        [YamlMember(Alias = "oneOf")]
        public List<ISchemaObject>? OneOf { get; set; }

        [YamlMember(Alias = "anyOf")]
        public List<ISchemaObject>? AnyOf { get; set; }

        [YamlMember(Alias = "description")]
        public string? Description { get; set; }

        [YamlMember(Alias = "default")]
        public object? Default { get; set; }

        [YamlMember(Alias = "additionalProperties")]
        public object? AdditionalProperties { get; set; }
        
        [YamlMember(Alias = "discriminator")]
        public IDiscriminatorObject? Discriminator { get; set; }
        
        [YamlMember(Alias = "example")]
        public object? Example { get; set; }

        [YamlMember(Alias = "examples")]
        public Dictionary<string, IExampleObject>? Examples { get; set; }
    
        [YamlIgnore] public Dictionary<string, object> VendorExtensions => _vendorExtensions;

        [YamlIgnore] public object? this[string key]
        {
            get => _vendorExtensions?.GetValueOrDefault(key) ?? null;
            set
            {
                if (value != null && key.StartsWith("x-"))
                {
                    _vendorExtensions[key] = value;
                }
            }
        }

        [YamlIgnore] public string? Name { get; set; }

    }
}