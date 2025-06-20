using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SwaggerToCode.Models // You can change this namespace
{
    public class GenerateConfig
    {
        [JsonPropertyName("swagger")]
        public List<string> Swagger { get; set; }

        [JsonPropertyName("templates")]
        public List<TemplateConfig> Templates { get; set; }

        [JsonPropertyName("root-paths")]
        public List<RootPathConfig> RootPaths { get; set; }

        [JsonPropertyName("meta-data")]
        public string MetaData { get; set; }
        
        [JsonIgnore]
        public List<string> SwaggerPaths => 
            Swagger.Select(swagger => Path.Combine(GetSwaggerRootPath(), swagger)).ToList();

        [JsonIgnore]
        public List<TemplateConfig> TemplateConfigs =>
             Templates.Where(t => t.Use).ToList();

        [JsonIgnore]
        public string TemplateRootPath => GetRootPath("Template");

        static public GenerateConfig? FromJsonFile(string strJsonFileName)
        {
            try
            {
                using (TextReader reader = new StreamReader(strJsonFileName))
                {
                    string strJson = reader.ReadToEnd();
                    return GenerateConfig.FromJson(strJson);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        static public GenerateConfig FromJson(string strJson)
        {
            return JsonSerializer.Deserialize<GenerateConfig>(strJson);
        }

        public bool ToJsonFile(string strJsonFileName)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(strJsonFileName))
                {
                    writer.Write(ToJson());
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
        
        private string GetSwaggerRootPath()
        {
            return GetRootPath("Swagger");
        }

        public string GetRootPath(string strPathKey)
        {
            return RootPaths
                .FirstOrDefault(root => root.Name == strPathKey)
                ?.Path ?? string.Empty;
        }
    }

    public class TemplateConfig
    {
        [JsonPropertyName("template")]
        public string Template { get; set; }

        [JsonPropertyName("file-extension")]
        public string FileExtension { get; set; }

        [JsonPropertyName("path-root")]
        public string PathRoot { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("target")]
        public string Target { get; set; }

        [JsonPropertyName("generate-type")]
        public string GenerateType { get; set; }

        [JsonPropertyName("generate-params")]
        public List<string> GenerateParams { get; set; }

        [JsonPropertyName("use")]
        public bool Use { get; set; }
    }

    public class RootPathConfig
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }
    }
}