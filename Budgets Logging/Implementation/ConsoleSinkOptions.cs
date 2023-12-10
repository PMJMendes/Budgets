using Serilog.Events;

namespace Krypton.Budgets.Logging.Implementation;

internal class ConsoleSinkOptions : SinkOptions
{
    public LogEventLevel? StdErrorRedirect { get; set; }
}
