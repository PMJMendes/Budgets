using Krypton.Budgets.Blazor.APIClient._Common;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Krypton.Budgets.Blazor.APIClient._Base;

public abstract class ServiceBase
{
    protected static readonly JsonSerializerOptions JSON_OPTIONS = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected readonly HttpClient client;
    protected readonly string url;

    public ServiceBase(HttpClient client, string url)
    {
        this.client = client;
        this.url = "/api/" + url;
    }

    protected static async Task<SafeResult<R>> HandleHttpResponse<R>(HttpResponseMessage response) =>
        response.StatusCode switch
        {
            HttpStatusCode.OK => new SafeResult<R>(await
                    response.Content.ReadFromJsonAsync<R>(JSON_OPTIONS)),
            HttpStatusCode.BadRequest => new SafeResult<R>(await
                    response.Content.ReadFromJsonAsync<ErrorResults>(JSON_OPTIONS)),
            HttpStatusCode.NotFound => new SafeResult<R>(ErrorResults.NOT_FOUND),
            HttpStatusCode.Unauthorized => new SafeResult<R>(ErrorResults.UNAUTHORIZED),
            HttpStatusCode.Forbidden => new SafeResult<R>(ErrorResults.FORBIDDEN),
            _ => new SafeResult<R>(ErrorResults.UNKNOWN_ERROR)
        };
}
