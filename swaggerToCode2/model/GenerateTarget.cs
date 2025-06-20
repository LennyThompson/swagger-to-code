namespace SwaggerToCode.Models;

public interface TemplateParameter
{
    string Name { get; }
    object Model { get; }
}

public interface GenerateTarget
{
    string TargetName { get; }
    string TargetType { get; }
    Dictionary<string, TemplateParameter> Parameters { get; }
}
