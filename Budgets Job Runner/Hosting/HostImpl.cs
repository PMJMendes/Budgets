using Krypton.Budgets.JobRunner.Implementation;
using System.Security.Claims;

namespace Krypton.Budgets.JobRunner.Hosting;

internal class HostImpl : Domain._Ports.Hosting.IHost
{
    private readonly QuartzUser user;
    private readonly ClaimsPrincipal principsl;
    private readonly string traceId;

    public HostImpl()
    {
        user = new QuartzUser();
        principsl = new ClaimsPrincipal(user);
        traceId = Guid.NewGuid().ToString();
    }

    public ClaimsPrincipal User
    {
        get
        {
            user.JobName = "quartz: " + JobListener.CurrentJob;
            return principsl;
        }
    }

    public string CorrelationId => traceId;
}
