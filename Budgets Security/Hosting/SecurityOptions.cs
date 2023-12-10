namespace Krypton.Budgets.Security.Hosting;

public class SecurityOptions
{
    public string? Authority { get; set; }
    public string? Audience { get; set; }
    public string? MetadataURL { get; set; }
    public bool? IsDevelopment { get; set; }
}
