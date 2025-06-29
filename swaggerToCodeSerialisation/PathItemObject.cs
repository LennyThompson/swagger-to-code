using YamlDotNet.Serialization;

namespace OpenApi.Models
{

    public class PathItemObject : IPathItemObject
    {
        [YamlMember(Alias = "summary")] public string Summary { get; set; }

        [YamlMember(Alias = "description")] public string Description { get; set; }

        [YamlMember(Alias = "get")] public IOperationObject Get { get; set; }

        [YamlMember(Alias = "put")] public IOperationObject Put { get; set; }

        [YamlMember(Alias = "post")] public IOperationObject Post { get; set; }

        [YamlMember(Alias = "delete")] public IOperationObject Delete { get; set; }

        [YamlMember(Alias = "parameters")] public List<IParameterObject> Parameters { get; set; }
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
        public List<IParameterObject> Parameters { get; set; }

        [YamlMember(Alias = "requestBody")]
        public IRequestBodyObject RequestBody { get; set; }

        [YamlMember(Alias = "responses")]
        public Dictionary<string, IResponseObject> Responses { get; set; }

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
        public ISchemaObject Schema { get; set; }
        [YamlMember(Alias = "example")]
        public object? Example { get; set; }

        public Dictionary<string, IExampleObject>? Examples { get; set; }
    }

    public class RequestBodyObject : IRequestBodyObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "required")]
        public bool Required { get; set; }

        [YamlMember(Alias = "content")]
        public Dictionary<string, IMediaTypeObject> Content { get; set; }
    }

    public class ResponseObject : IResponseObject
    {
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        [YamlMember(Alias = "content")]
        public Dictionary<string, IMediaTypeObject> Content { get; set; }

        [YamlMember(Alias = "headers")]
        public Dictionary<string, IHeaderObject> Headers { get; set; }
    }

}