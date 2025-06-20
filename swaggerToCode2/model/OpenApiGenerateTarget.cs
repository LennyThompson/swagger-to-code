using OpenApi.Models;

namespace SwaggerToCode.Models;

public class TemplateParameterImpl : TemplateParameter
{
    private string _strName;
    private object _model;
    public TemplateParameterImpl(string strName, object model)
    {
        _strName = strName;
        _model = model;
    }

    public string Name => _strName;
    public object Model => _model;
}

public class OpenApiGenerateTarget : GenerateTarget
{
    private string _strName;
    private string _strType;
    private Dictionary<string, TemplateParameter> _mapTemplateParams;
    
    public OpenApiGenerateTarget(string strName, OpenApiDocument doc, IPathItemObject pathItemTarget)
    {
        _strName = strName;
        _strType = "each-path";
        _mapTemplateParams = new Dictionary<string, TemplateParameter>();
        _mapTemplateParams["schema"] = new TemplateParameterImpl("schema", doc);
        _mapTemplateParams = new Dictionary<string, TemplateParameter>();
        _mapTemplateParams["path"] = new TemplateParameterImpl("path", pathItemTarget);
    }

    public OpenApiGenerateTarget(string strName, OpenApiDocument doc)
    {
        _strName = strName;
        _strType = "all";
        _mapTemplateParams = new Dictionary<string, TemplateParameter>();
        _mapTemplateParams["schema"] = new TemplateParameterImpl("schema", doc);
        _mapTemplateParams = new Dictionary<string, TemplateParameter>();
        _mapTemplateParams["paths"] = new TemplateParameterImpl("paths", doc.Paths.Values);
        _mapTemplateParams = new Dictionary<string, TemplateParameter>();
        _mapTemplateParams["models"] = new TemplateParameterImpl("models", doc.Components.Schemas.Values);
    }

    public OpenApiGenerateTarget(string strName, OpenApiDocument doc, ISchemaObject schemaObject)
    {
        _strName = strName;
        _strType = "each-model";
        _mapTemplateParams = new Dictionary<string, TemplateParameter>();
        _mapTemplateParams["schema"] = new TemplateParameterImpl("schema", doc);
        _mapTemplateParams = new Dictionary<string, TemplateParameter>();
        _mapTemplateParams["model"] = new TemplateParameterImpl("model", schemaObject);
    }

    public string TargetName => _strName;
    public string TargetType => _strType;
    public Dictionary<string, TemplateParameter> Parameters => _mapTemplateParams;
}