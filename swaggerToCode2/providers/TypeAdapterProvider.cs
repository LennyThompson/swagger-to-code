using OpenApi.Models;
using SwaggerToCode.Adapters;
using SwaggerToCode.Models;

namespace swaggerToCode2.providers;

public interface TypeAdapterProvider
{
    TypeAdapter GetTypeAdapter(TemplateConfig templateConfigFor, string strName, SchemaObject schemaObj);
}