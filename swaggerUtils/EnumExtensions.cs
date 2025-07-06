using System.Reflection;
namespace swagger.utils;

[AttributeUsage(AttributeTargets.Field)]
public class StringValueAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}

public static class EnumExtensions
{
    public static string GetStringValue(this Enum value)
    {
        // Get the type
        Type type = value.GetType();
        
        // Get the field info
        FieldInfo? fieldInfo = type.GetField(value.ToString());
        
        // Get the StringValue attributes
        StringValueAttribute[] attributes = 
            fieldInfo?.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[] ?? [];
        
        // Return the first if found, otherwise the enum's name
        return attributes.Length > 0 ? attributes[0].Value : throw new ArgumentException($"No string defined for enum value {value}");
    }
    
    // You can also add a method to get the enum from a string value
    public static T GetEnumFromStringValue<T>(string stringValue) where T : Enum
    {
        foreach (T enumValue in Enum.GetValues(typeof(T)))
        {
            if (enumValue.GetStringValue() == stringValue)
            {
                return enumValue;
            }
        }
        
        // If not found, you can either throw an exception or return a default value
        throw new ArgumentException($"No enum value with string value '{stringValue}' found.");
    }
}
