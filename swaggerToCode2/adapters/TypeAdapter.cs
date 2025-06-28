using System.Text;
using OpenApi.Models;

namespace SwaggerToCode.Adapters;

public interface NameConverter
{
    string Name { get; }
    string ConvertedName { get; }
}

public class SnakeToCamelCaseAdapter : NameConverter
{
    private string _strName;
    public SnakeToCamelCaseAdapter(string strFrom)
    {
        _strName = strFrom;
    }

    public string Name => _strName;
    public string ConvertedName => FromSnakeCase();

    private string FromSnakeCase()
    {
        return FromCase('-');
    }
    protected string FromCase(char separator)
    {
        if (string.IsNullOrEmpty(Name) || !Name.Contains(separator))
        {
            return Name;
        }

        // Split the string by underscores
        string[] parts = Name.Split(separator);
    
        // Convert the first part to lowercase
        var camelCase = parts[0].ToLowerInvariant();
    
        // Capitalize the first letter of each remaining part and append
        for (int i = 1; i < parts.Length; i++)
        {
            if (!string.IsNullOrEmpty(parts[i]))
            {
                camelCase += char.ToUpperInvariant(parts[i][0]) + 
                             (parts[i].Length > 1 ? parts[i].Substring(1).ToLowerInvariant() : "");
            }
        }
    
        return camelCase;

    }
    
}

public class BellySnakeToCamelCaseApapter : SnakeToCamelCaseAdapter
{
    private string _strName;
    public BellySnakeToCamelCaseApapter(string strFrom) : base(strFrom)
    {
    }

    public string ConvertedName => FromCase('_');
}

public class CamelToSnakeCaseAdapter : NameConverter
{
    private string _strName;

    public CamelToSnakeCaseAdapter(string strFrom)
    {
        _strName = strFrom;
    }

    public string Name => _strName;
    public string ConvertedName => CamelToCase('-');

    protected string CamelToCase(char separator)
    {
        if (string.IsNullOrEmpty(Name))
        {
            return Name;
        }

        StringBuilder result = new StringBuilder();
        bool isPreviousCharUpper = false;
        bool isCurrentCharUpper;
        bool isNextCharLower = false;

        // Process each character
        for (int i = 0; i < Name.Length; i++)
        {
            char currentChar = Name[i];
            isCurrentCharUpper = char.IsUpper(currentChar);
        
            // Check if next char is lowercase (if it's not the last char)
            if (i + 1 < Name.Length)
            {
                isNextCharLower = char.IsLower(Name[i + 1]);
            }

            // Add underscore in the following cases:
            // 1. Current char is upper and previous char is lower (e.g., "aB" -> "a_b")
            // 2. Current char is upper, previous char is upper, and next char is lower (e.g., "ABc" -> "a_bc")
            if (isCurrentCharUpper)
            {
                if ((i > 0 && !isPreviousCharUpper) || 
                    (isPreviousCharUpper && isNextCharLower))
                {
                    result.Append(separator);
                }
                result.Append(char.ToLowerInvariant(currentChar));
            }
            else
            {
                result.Append(currentChar);
            }

            isPreviousCharUpper = isCurrentCharUpper;
        }

        return result.ToString();
    }

}

public class CamelToBellySnakeCaseAdapter : CamelToSnakeCaseAdapter
{
    public CamelToBellySnakeCaseAdapter(string strFrom) : base(strFrom)
    {
    }

    public string ConvertedName => CamelToCase('_');

}

public class CamelToPascalCaseAdapter : NameConverter
{
    private NameConverter _baseConverter;

    public CamelToPascalCaseAdapter(NameConverter baseConverter)
    {
        _baseConverter = baseConverter;
    }

    public string Name => _baseConverter.Name;
    public string ConvertedName => ToPascalCase();

    private string ToPascalCase()
    {
        string strBaseConverted = _baseConverter.ConvertedName;
        if (string.IsNullOrEmpty(strBaseConverted))
        {
            return strBaseConverted;
        }

        return char.ToUpperInvariant(strBaseConverted[0]) + strBaseConverted.Substring(1);
    }

}

public delegate NameConverter NameConverterBuilder(string strName);

public class PascalToCamelCaseAdapter : NameConverter
{
    private string _strName;
    private NameConverterBuilder _converterBuilder;

    public PascalToCamelCaseAdapter(string strFrom, NameConverterBuilder converterBuilder)
    {
        _strName = strFrom;
        _converterBuilder = converterBuilder;
    }

    public string Name => _strName;
    public string ConvertedName => FromPascalCase();

    private string FromPascalCase()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return Name;
        }
        string strBaseConverted = char.ToUpperInvariant(Name[0]) + Name.Substring(1);;

        return _converterBuilder(strBaseConverted).ConvertedName;
    }

}

public interface TypeAdapter
{
    string Name { get; }
    string OutputName { get; }
    string TypeName { get; }
    string ListTypeName { get; }
    string MemberPrefix { get; }
}

public class CppTypeAdapter : TypeAdapter
{
    private string _strName;
    private string _strSwaggerType;
    private string? _strSwaggerFormat;
    private ISchemaObject? _schemaObject;
    private NameConverterBuilder _converter;

    public CppTypeAdapter(string strName, string strSwaggerType, string strSwaggerFormat, NameConverterBuilder nameConverter, ISchemaObject? schemaObject)
    {
        _strName = strName;
        _strSwaggerType = strSwaggerType;
        _schemaObject = schemaObject;
        _strSwaggerFormat = strSwaggerFormat;
        _converter = nameConverter;
    }


    public string Name => _strName;
    public string OutputName => _converter(Name).ConvertedName;

    public string TypeName => SwaggerTypeToCpp();
    public string ListTypeName => $"std::vector<{TypeName}>";
    public string Prefix => SwaggerTypeToCppPrefix();
    public string MemberPrefix => $"_{Prefix}";

    public bool IsComponentType => _schemaObject != null && !_schemaObject.IsSimpleType;
    
    private string SwaggerTypeToCpp()
    {
        if (IsComponentType)
        {
            return _converter(_schemaObject!.Name).ConvertedName;
        }
        switch (_strSwaggerType)
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
        switch (_strSwaggerType)
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