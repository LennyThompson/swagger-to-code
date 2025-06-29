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
        public Dictionary<string, ISchemaObject>? Properties { get; set; }

        [YamlMember(Alias = "items")]
        public ISchemaObject? Items { get; set; }

        [YamlMember(Alias = "required")]
        public List<string> Required { get; set; }

        [YamlMember(Alias = "enum")]
        public List<string> Enum { get; set; }

        [YamlMember(Alias = "allOf")]
        public List<ISchemaObject>? AllOf { get; set; }

        [YamlMember(Alias = "oneOf")]
        public List<ISchemaObject>? OneOf { get; set; }

        [YamlMember(Alias = "anyOf")]
        public List<ISchemaObject>? AnyOf { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "default")]
        public object Default { get; set; }

        [YamlMember(Alias = "additionalProperties")]
        public object AdditionalProperties { get; set; }
        
        [YamlMember(Alias = "discriminator")]
        public DiscriminatorObject Discriminator { get; set; }
        
        [YamlMember(Alias = "example")]
        public object? Example { get; set; }

        [YamlMember(Alias = "examples")]
        public Dictionary<string, IExampleObject>? Examples { get; set; }
    
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
        [YamlIgnore] public bool IsObjectType => Type == "object" || (IsArrayType && Items?.IsObjectType == true);
        [YamlIgnore] public bool IsReference => ReferenceSchemaObject != null || !string.IsNullOrEmpty(Ref);
        [YamlIgnore] public bool IsSimpleType => Type == "string" || Type == "number" || Type == "integer" || Type == "boolean"; // TODO or Type != "object";???
        [YamlIgnore] public ISchemaObject? ReferenceSchemaObject { get; set; }
        [YamlIgnore] public List<ISchemaObjectField> Fields => IsReference ? 
            ReferenceSchemaObject.Fields : 
            Properties?.Select(property => new SchemaObjectField(this, property.Key, property.Value) as ISchemaObjectField).ToList() ?? new List<ISchemaObjectField>();

        public bool UpdateSchemaReferences(ISchemaObjectFinder finder)
        {
            if (IsReference)
            {
                ReferenceSchemaObject = finder.FindSchemaByReference(Ref);
            }
            else if (IsArrayType && Items != null)
            {
                Items.UpdateSchemaReferences(finder);
            }
            
            if (OneOf != null && OneOf.Count > 0)
            {
                foreach (ISchemaObject schemaOf in OneOf)
                {
                    schemaOf.UpdateSchemaReferences(finder);
                }
            }
            if (AnyOf != null && AnyOf.Count > 0)
            {
                foreach (ISchemaObject schemaOf in AnyOf)
                {
                    schemaOf.UpdateSchemaReferences(finder);
                }
            }
            if (AllOf != null && AllOf.Count > 0)
            {
                foreach (ISchemaObject schemaOf in AllOf)
                {
                    schemaOf.UpdateSchemaReferences(finder);
                }
            }

            if (Properties != null)
            {
                foreach (var property in Properties)
                {
                    property.Value.UpdateSchemaReferences(finder);
                }

            }

            return true;
        }

        [YamlIgnore] public bool IsArrayType => Type == "array";

        [YamlIgnore]
        public List<ISchemaObject> References
        {
            get
            {
                List<ISchemaObject> listReferences = new List<ISchemaObject>();
                if (IsReference)
                {
                    listReferences.Add(this);
                }

                if (IsArrayType && Items?.IsReference == true)
                {
                    listReferences.Add(this);
                }
                listReferences.AddRange
                (
                    Properties?.Values.SelectMany(prop => prop.References).ToList() ?? new List<ISchemaObject>()
                );
                if (AnyOf?.Any(schemaObj => schemaObj.IsReference) ?? false)
                {
                    listReferences.AddRange(AnyOf.SelectMany(anyOf => anyOf.References).ToList());
                }
                if(AllOf?.Any(schemaObj => schemaObj.IsReference) ?? false)
                {
                    listReferences.AddRange(AllOf.SelectMany(anyOf => anyOf.References).ToList());
                }
                if(OneOf?.Any(schemaObj => schemaObj.IsReference) ?? false)
                {
                    listReferences.AddRange(OneOf.SelectMany(anyOf => anyOf.References).ToList());
                }

                return listReferences;
            }
        }
    }

    public class SchemaObjectField : ISchemaObjectField
    {
        private ISchemaObject _parent;
        private ISchemaObject _field;
        private string _strName;
        public SchemaObjectField(ISchemaObject objParent, string strName, ISchemaObject property)
        {
            _parent = objParent;
            _strName = strName;
            _field = property;
        }
        
        public ISchemaObject Parent => _parent;
        public string Name => _strName;
        public ISchemaObject Field => _field;

        public List<ISchemaObject>? References => _field.References;
    }

}