using Serilog.Events;

namespace Krypton.Budgets.Logging.Implementation;

internal class SinkOptions
{
    public LogEventLevel Level { get; set; } = LogEventLevel.Verbose;
}
