using Antlr4.StringTemplate;

namespace SwaggerToCode.Services
{

    public interface ITemplateManagerService
    {
        TemplateGroup? LoadTemplates();
        Template? GetTemplate(string templateName);
        string RenderTemplate<T>(string templateName, T model) where T : class;
    }
}