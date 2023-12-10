using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.APIClient._Impl;
using System.Net.Http.Json;

namespace Krypton.Budgets.Blazor.APIClient._Base;

public abstract class PostServiceBase<A, R> : ServiceBase
{
    public PostServiceBase(HttpClient client, string url) : base(client, url) { }

    protected async Task<SafeResult<R>> PostAsync(A payload)
    {
        var response = await client.PostAsJsonAsync(url, payload);
        return await HandleHttpResponse<R>(response);
    }
}

public abstract class PostServiceBase<R> : PostServiceBase<EmptyPostArgs, R>
{
    public PostServiceBase(HttpClient client, string url) : base(client, url) { }

    protected async Task<SafeResult<R>> PostAsync() =>
        await PostAsync(EmptyPostArgs.Instance);
}
