using System.Security.Claims;
using IHost = Krypton.Budgets.Domain._Ports.Hosting.IHost;

namespace Krypton.Budgets.API._Hosting;

internal class HostImpl : IHost
{
    private readonly HttpContext? context;

    public HostImpl(IHttpContextAccessor accessor)
    {
        context = accessor.HttpContext;
    }

    public ClaimsPrincipal User => context?.User ?? new ClaimsPrincipal();

    public string CorrelationId => context?.TraceIdentifier ?? "";
}
