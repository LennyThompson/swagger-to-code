
using System.Text;

namespace swagger.utils.converters;

public interface NameConverter
{
    string Name { get; }
    string ConvertedName { get; }
}

public class SnakeToCamelCaseAdapter(string strFrom) : NameConverter
{
    public string Name => strFrom;
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

public class BellySnakeToCamelCaseAdapter(string strFrom) : SnakeToCamelCaseAdapter(strFrom)
{
    public new string ConvertedName => FromCase('_');
}

public class CamelToSnakeCaseAdapter(string strFrom) : NameConverter
{
    public string Name { get; } = strFrom;

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

public class CamelToBellySnakeCaseAdapter(string strFrom) : CamelToSnakeCaseAdapter(strFrom)
{
    public new string ConvertedName => CamelToCase('_');

}

public class CamelToPascalCaseAdapter(NameConverter baseConverter) : NameConverter
{
    public string Name => baseConverter.Name;
    public string ConvertedName => ToPascalCase();

    private string ToPascalCase()
    {
        string strBaseConverted = baseConverter.ConvertedName;
        if (string.IsNullOrEmpty(strBaseConverted))
        {
            return strBaseConverted;
        }

        return char.ToUpperInvariant(strBaseConverted[0]) + strBaseConverted.Substring(1);
    }

}

public delegate NameConverter NameConverterBuilder(string strName);

public class PascalToCamelCaseAdapter(string strFrom, NameConverterBuilder converterBuilder) : NameConverter
{
    public string Name { get; } = strFrom;

    public string ConvertedName => FromPascalCase();

    private string FromPascalCase()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return Name;
        }
        string strBaseConverted = char.ToUpperInvariant(Name[0]) + Name.Substring(1);;

        return converterBuilder(strBaseConverted).ConvertedName;
    }

}
