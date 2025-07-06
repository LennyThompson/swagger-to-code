using System.Text;
using OpenApi.Models;
using swagger.utils.converters;

namespace SwaggerToCode.Adapters;


public interface TypeAdapter
{
    public static List<SchemaObjectType> SimpleTypes { get; } = new() { SchemaObjectType.Boolean, SchemaObjectType.Integer, SchemaObjectType.Number, SchemaObjectType.String };
    string? Name { get; }
    string OutputName { get; }
    string TypeName { get; }
    string ListTypeName { get; }
    string MemberPrefix { get; }
    bool IsSimpleType { get; }
    bool IsObjectType { get; }
    bool IsArrayType { get; }
    bool IsCompositeType { get; }
    bool IsEnumType { get; }
}

public class CppTypeAdapter(
    string strName,
    string strSwaggerType,
    string strSwaggerFormat,
    NameConverterBuilder nameConverter,
    ISchemaObject? schemaObject)
    : TypeAdapter
{
    private readonly string? _strSwaggerFormat = strSwaggerFormat;


    public string Name => strName;
    public string OutputName => nameConverter(Name).ConvertedName;

    public string TypeName => SwaggerTypeToCpp();
    public string ListTypeName => $"std::vector<{TypeName}>";
    public string Prefix => SwaggerTypeToCppPrefix();
    public string MemberPrefix => $"_{Prefix}";

    public bool IsSimpleType => schemaObject != null 
                                   && 
                                   (
                                       TypeAdapter.SimpleTypes.Contains( schemaObject.ObjectType)
                                       ||
                                       (IsArrayType && (schemaObject.Items?.ObjectType == SchemaObjectType.Object))
                                    );

    public bool IsObjectType => schemaObject != null 
                                && 
                                (
                                    schemaObject.ObjectType == SchemaObjectType.Object
                                    ||
                                    (IsArrayType && (schemaObject.Items?.ObjectType == SchemaObjectType.Object))
                                );

    public bool IsArrayType => schemaObject is { ObjectType: SchemaObjectType.Array };

    public bool IsCompositeType => schemaObject is CompositeSchemaObject;

    public bool IsEnumType => schemaObject is { Enum.Count: > 0 };
    
    private string SwaggerTypeToCpp()
    {
        if (IsObjectType)
        {
            Console.WriteLine($"Type is component type {schemaObject!.Name}");
            return nameConverter(schemaObject!.Name!).ConvertedName;
        }
        switch (strSwaggerType)
        {
            case "string":
                if (string.IsNullOrEmpty(_strSwaggerFormat))
                {
                    return "std::string";
                }
                switch (_strSwaggerFormat)
                {
                    case "date-time":
                        return "std::chrono::system_clock::time_point";
                    // TODO: add case for binary etc...
                    default:
                        return "std::string";
                }
            case "number":
                switch (_strSwaggerFormat)
                {
                    case "int32":
                        return "int";
                    case "int64":
                        return "int64_t";
                    case "float":
                        return "float";
                    case "double":
                        return "double";
                    default:
                        return "int";
                }
            case "boolean":
                return "bool";
            case "array":
                return "std::vector<>";
            case "object":
                return "int";
            case "integer":
                return "int";
            case "null":
                return "nullptr";
            default:
                return "nullptr";
        }
    }
    private string SwaggerTypeToCppPrefix()
    {
        switch (strSwaggerType)
        {
            case "string":
                switch (_strSwaggerFormat)
                {
                    case "date-time":
                        return "time";
                    // TODO: add case for binary etc...
                    default:
                        return "str";
                }
            case "number":
                switch (_strSwaggerFormat)
                {
                    case "int32":
                        return "n";
                    case "int64":
                        return "n";
                    case "float":
                        return "f";
                    case "double":
                        return "d";
                    default:
                        return "n";
                }
            case "boolean":
                return "b";
            case "array":
                return "arr";
            case "object":
                return "p";
            case "integer":
                return "n";
            case "null":
                return "";
            default:
                return "";
        }
    }
}