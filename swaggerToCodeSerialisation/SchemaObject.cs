using YamlDotNet.Serialization;

namespace OpenApi.Models
{

    public class SchemaObject : ISchemaObject
    {
        private readonly Dictionary<string, object> _vendorExtensions = new();

        [YamlMember(Alias = "$ref")]
        public string Ref { get; set; }

        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        [YamlMember(Alias = "format")]
        public string Format { get; set; }

        [YamlMember(Alias = "properties")]
        public Dictionary<string, SchemaObject> Properties { get; set; }

        [YamlMember(Alias = "items")]
        public SchemaObject Items { get; set; }

        [YamlMember(Alias = "required")]
        public List<string> Required { get; set; }

        [YamlMember(Alias = "enum")]
        public List<string> Enum { get; set; }

        [YamlMember(Alias = "allOf")]
        public List<SchemaObject> AllOf { get; set; }

        [YamlMember(Alias = "oneOf")]
        public List<SchemaObject> OneOf { get; set; }

        [YamlMember(Alias = "anyOf")]
        public List<SchemaObject> AnyOf { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "default")]
        public object Default { get; set; }

        [YamlMember(Alias = "additionalProperties")]
        public object AdditionalProperties { get; set; }
        
        [YamlMember(Alias = "discriminator")]
        public DiscriminatorObject Discriminator { get; set; }
        
        [YamlIgnore] public Dictionary<string, object> VendorExtensions => _vendorExtensions;

        [YamlIgnore] public object this[string key]
        {
            get => _vendorExtensions.TryGetValue(key, out var value) ? value : null;
            set
            {
                if (key.StartsWith("x-"))
                {
                    _vendorExtensions[key] = value;
                }
            }
        }

        [YamlIgnore] public string Name { get; set; }
        [YamlIgnore] public bool IsReference => ReferenceSchemaObject != null || !string.IsNullOrEmpty(Ref);
        [YamlIgnore] public bool IsSimpleType => Type == "string" || Type == "number" || Type == "integer" || Type == "boolean"; // TODO or Type != "object";???
        [YamlIgnore] public SchemaObject? ReferenceSchemaObject { get; set; }
        [YamlIgnore] public List<ISchemaObjectField> Fields => IsReference ? 
            ReferenceSchemaObject.Fields : 
            Properties?.Keys.Select(propName => new SchemaObjectField(this, propName, Properties[propName]) as ISchemaObjectField).ToList() ?? new List<ISchemaObjectField>();

        [YamlIgnore] public bool IsArrayType => Type == "array";
    }

    public class SchemaObjectField : ISchemaObjectField
    {
        private SchemaObject _parent;
        private SchemaObject _field;
        private string _strName;
        public SchemaObjectField(SchemaObject objParent, string strName, SchemaObject property)
        {
            _parent = objParent;
            _strName = strName;
            _field = property;
        }

        public ISchemaObject Parent => _parent;
        public string Name => _strName;
        public ISchemaObject Field => _field;
    }

}