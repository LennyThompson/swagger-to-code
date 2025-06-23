using SwaggerToCode.Models;

namespace swaggerToCode2.providers;

public interface OutputFileProvider
{
    bool WriteToFile(GenerateTarget generateTarget, string strTargetName, string strRendered);
}