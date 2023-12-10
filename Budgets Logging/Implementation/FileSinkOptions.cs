using Serilog;

namespace Krypton.Budgets.Logging.Implementation;

internal class FileSinkOptions : SinkOptions
{
    public string Prefix { get; set; } = "Budgets.";
    public long SizeLimit { get; set; } = 1L * 1024 * 1024 * 1024;
    public TimeSpan? FlushInterval { get; set; }
    public RollingInterval Rolling { get; set; } = RollingInterval.Infinite;
    public int? FileCountLimit { get; set; }
    public TimeSpan? FileTimeLimit { get; set; }
}
