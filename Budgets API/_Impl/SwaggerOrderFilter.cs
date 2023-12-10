using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Krypton.Budgets.API._Impl;

internal class SwaggerOrderFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = swaggerDoc.Paths.
            OrderBy(e => e.Value.Operations.Values.FirstOrDefault()?.Tags.FirstOrDefault()?.Name?.Replace("Global", "_")).
            ThenBy(e => e.Value.Operations.Keys.FirstOrDefault()).
            ThenBy(e => e.Key).
            ToList();

        swaggerDoc.Paths.Clear();
        foreach (var path in paths)
        {
            swaggerDoc.Paths.Add(path.Key, path.Value);
        }
    }
}
