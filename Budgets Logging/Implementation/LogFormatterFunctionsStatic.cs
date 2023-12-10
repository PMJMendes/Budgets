using Serilog.Events;

namespace Krypton.Budgets.Logging.Implementation;

internal static class LogFormatterFunctionsStatic
{
    public static LogEventPropertyValue? ShortenPath(LogEventPropertyValue? path)
    {
        if (path is ScalarValue sv && sv.Value is string s)
        {
            var array = s.Split('.');
            var len = array.Length;

            return len < 2 ? path :
                new ScalarValue(string.Join(".", array.Select((w, i) => (i == 0 || i == len - 1) ? w : w[..1])));
        }

        return path;
    }
}
