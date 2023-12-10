namespace Krypton.Budgets.Logging.Implementation;

internal class GelfSinkOptions : SinkOptions
{
    public Uri Host { get; set; } = new Uri("http://localhost");
    public string? Username { get; set; }
    public string? Password { get; set; }
}
