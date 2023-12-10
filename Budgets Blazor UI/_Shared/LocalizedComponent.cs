using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SoloX.BlazorJsonLocalization;

namespace Krypton.Budgets.Blazor.UI._Shared;

public abstract class LocalizedComponent<TSource> : ComponentBase
{
    [Inject]
    protected IStringLocalizer<TSource> L { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await L.LoadAsync();
    }
}
