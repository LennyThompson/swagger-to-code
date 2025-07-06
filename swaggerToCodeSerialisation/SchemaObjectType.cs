using swagger.utils;

namespace OpenApi.Models;

public enum SchemaObjectType
{
    [StringValue("other")]
    Unknown, 
    [StringValue("null")]
    Null, 
    [StringValue("string")]
    String, 
    [StringValue("number")]
    Number, 
    [StringValue("integer")]
    Integer, 
    [StringValue("boolean")]
    Boolean, 
    [StringValue("object")]
    Object, 
    [StringValue("array")]
    Array
}