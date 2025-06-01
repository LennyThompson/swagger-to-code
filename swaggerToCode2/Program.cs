using System.CommandLine;
using OpenApi.Models;

namespace swaggerToCode;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var fileOption = new Option<FileInfo?>(
                name: "--file",
                description: "The swagger/openapi file to process")
            { IsRequired = true };

        var outputOption = new Option<string>(
            name: "--output",
            description: "Output directory for generated code",
            getDefaultValue: () => Directory.GetCurrentDirectory());

        var rootCommand = new RootCommand("Swagger/OpenAPI to Code Generator")
        {
            fileOption,
            outputOption
        };

        rootCommand.SetHandler(async (file, output) =>
        {
            if (file == null)
            {
                Console.Error.WriteLine("Input file is required");
                return;
            }

            // TODO: Add your processing logic here
            Console.WriteLine($"Processing file: {file.FullName}");
            Console.WriteLine($"Output directory: {output}");

            using (TextReader reader = new StreamReader(file.FullName))
            {
                Console.WriteLine($"File: {file.FullName} - exists");
                String strContent = reader.ReadToEnd();
                Console.WriteLine(strContent);
                OpenApiDocument docYaml = OpenApiYamlSerializer.DeserializeFromYaml(strContent);

                docYaml.UpdateSchemaReferences();
                
                Console.WriteLine($"File: {file.FullName} - contains {docYaml.Paths.Count} paths");
            }

        }, fileOption, outputOption);

        return await rootCommand.InvokeAsync(args);
    }
}