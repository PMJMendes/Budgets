using System.Security.Claims;

namespace Krypton.Budgets.JobRunner.Implementation;

internal class QuartzUser : ClaimsIdentity
{
    public string JobName { set => AddClaim(new Claim("preferred_username", value)); }

    public override bool IsAuthenticated => true;
}
