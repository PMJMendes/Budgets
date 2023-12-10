using System.Security.Claims;

namespace Krypton.Budgets.Domain._Ports.Hosting;

public interface IHost
{
    ClaimsPrincipal User { get; }

    string CorrelationId { get; }
}
