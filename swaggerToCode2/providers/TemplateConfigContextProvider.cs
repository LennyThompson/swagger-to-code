using OpenApi.Models;
using SwaggerToCode.Models;

namespace swaggerToCode2.providers
{
    public interface TemplateConfigContextProvider
    {
        bool IsContextSet { get; }
        TemplateConfig CurrentTemplateConfig { get; }
        IOpenApiDocument CurrentOpenApiDocument { get; }
        void SetContext(TemplateConfig templateConfig, IOpenApiDocument openApiDocument);
        void ClearContext();
    }
}