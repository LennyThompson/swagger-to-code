using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenApi.Models;
using SwaggerToCode.Models;
using SwaggerToCode.Services;

namespace SwaggerToCode.Services
{
    public interface IOpenApiDocumentProvider
    {
        IReadOnlyList<OpenApiDocument> Documents { get; }
        Task LoadDocumentsAsync();
        OpenApiDocument GetDocumentByIndex(int index);
        OpenApiDocument GetDocumentByFilename(string filename);
    }
}