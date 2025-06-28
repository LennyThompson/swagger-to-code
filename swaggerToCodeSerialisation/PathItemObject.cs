using YamlDotNet.Serialization;

namespace OpenApi.Models
{

    public class PathItemObject : IPathItemObject
    {
        [YamlMember(Alias = "summary")] public string Summary { get; set; }

        [YamlMember(Alias = "description")] public string Description { get; set; }

        [YamlMember(Alias = "get")] public OperationObject Get { get; set; }

        [YamlMember(Alias = "put")] public OperationObject Put { get; set; }

        [YamlMember(Alias = "post")] public OperationObject Post { get; set; }

        [YamlMember(Alias = "delete")] public OperationObject Delete { get; set; }

        [YamlMember(Alias = "parameters")] public List<ParameterObject> Parameters { get; set; }
    }

    public class OperationObject : IOperationObject
    {
        [YamlMember(Alias = "tags")]
        public List<string> Tags { get; set; }

        [YamlMember(Alias = "summary")]
        public string Summary { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "operationId")]
        public string OperationId { get; set; }

        [YamlMember(Alias = "parameters")]
        public List<ParameterObject> Parameters { get; set; }

        [YamlMember(Alias = "requestBody")]
        public RequestBodyObject RequestBody { get; set; }

        [YamlMember(Alias = "responses")]
        public Dictionary<string, ResponseObject> Responses { get; set; }

        [YamlMember(Alias = "security")]
        public List<Dictionary<string, List<string>>> Security { get; set; }
    }

    public class ParameterObject : IParameterObject
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "in")]
        public string In { get; set; }

        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "required")]
        public bool Required { get; set; }

        [YamlMember(Alias = "schema")]
        public SchemaObject Schema { get; set; }
    }

    public class RequestBodyObject : IRequestBodyObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "required")]
        public bool Required { get; set; }

        [YamlMember(Alias = "content")]
        public Dictionary<string, MediaTypeObject> Content { get; set; }
    }

    public class ResponseObject : IResponseObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "content")]
        public Dictionary<string, MediaTypeObject> Content { get; set; }

        [YamlMember(Alias = "headers")]
        public Dictionary<string, HeaderObject> Headers { get; set; }
    }

}