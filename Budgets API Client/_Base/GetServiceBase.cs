using Krypton.Budgets.Blazor.APIClient._Common;
using Krypton.Budgets.Blazor.APIClient._Impl;
using System.Web;

namespace Krypton.Budgets.Blazor.APIClient._Base;

public abstract class GetServiceBase<A, R> : ServiceBase where A : IQueryArgs
{
    public GetServiceBase(HttpClient client, string url) : base(client, url) { }

    protected async Task<SafeResult<IEnumerable<R?>>> GetAsync(A args)
    {
        var argString = string.Join('&', args.GetQueryString().
            Where(kv => kv.Value is not null).
            Select(kv => kv.Key + "=" + (kv.Value is string value ? HttpUtility.UrlEncode(value) : "")));

        var response = await client.GetAsync(url + "/?" + argString);
        return await HandleHttpResponse<IEnumerable<R?>>(response);
    }
}

public abstract class GetServiceBase<R> : GetServiceBase<EmptyQueryArgs, R>
{
    public GetServiceBase(HttpClient client, string url) : base(client, url) { }

    protected async Task<SafeResult<IEnumerable<R?>>> GetAsync() =>
        await GetAsync(EmptyQueryArgs.Instance);
}
