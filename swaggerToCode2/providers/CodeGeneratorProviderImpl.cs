using System.Reflection;
using SwaggerToCode;
using swaggerToCode.code_generators;

namespace swaggerToCode2.providers
{
    public class CodeGeneratorProviderImpl : CodeGeneratorProvider
    {
        private readonly Dictionary<string, CodeGenerator> _generators =
            new Dictionary<string, CodeGenerator>(StringComparer.OrdinalIgnoreCase);

        public CodeGeneratorProviderImpl()
        {
            // Auto-discover and register code generators from the current assembly
            DiscoverAndRegisterGenerators();
        }

        public CodeGenerator? GetGenerator(string generatorId)
        {
            if (string.IsNullOrEmpty(generatorId))
                return null;

            return _generators.TryGetValue(generatorId, out var generator)
                ? generator
                : null;
        }

        public Dictionary<string, CodeGenerator> AllGenerators => _generators;

        public void RegisterGenerator(CodeGenerator generator)
        {
            if (generator != null && !string.IsNullOrEmpty(generator.Name))
            {
                _generators[generator.Name] = generator;
            }
        }

        private void DiscoverAndRegisterGenerators()
        {
            try
            {
                // Get the current assembly
                var assembly = Assembly.GetExecutingAssembly();

                // Find all types that implement CodeGenerator
                var generatorTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && typeof(CodeGenerator).IsAssignableFrom(t))
                    .ToList();

                // Instantiate and register each code generator
                foreach (var generatorType in generatorTypes)
                {
                    try
                    {
                        // Create an instance of the generator
                        if (Activator.CreateInstance(generatorType) is CodeGenerator generator)
                        {
                            // Register the generator using its Name property
                            RegisterGenerator(generator);
                            Console.WriteLine($"Registered code generator: {generator.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating instance of {generatorType.Name}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Discovered and registered {_generators.Count} code generators");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error discovering code generators: {ex.Message}");
            }
        }
    }
}