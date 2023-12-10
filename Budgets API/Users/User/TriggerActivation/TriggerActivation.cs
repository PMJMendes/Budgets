using Krypton.Budgets.API._Base;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain.Users.User_.TriggerActivation;

namespace Krypton.Budgets.API.Users.User.TriggerActivation;

internal class TriggerActivation : PostEndpointBase<TriggerActivation, TriggerActivationResults, TargetArgs>
{
    private readonly ITriggerActivation op;

    public TriggerActivation(ITriggerActivation op, IHttpContextAccessor accessor, ILogger<TriggerActivation> logger) : base(accessor, logger)
    {
        this.op = op;
    }

    protected override Task<IResult> PostAsync(TargetArgs? input) => InRequestScope(() =>
        CallDomainOpAsync(op, input, r => new TriggerActivationResults(r)));
}
