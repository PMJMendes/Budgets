using Krypton.Budgets.Logging.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Expressions;
using Serilog.Extensions.Logging;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Helpers;
using Serilog.Sinks.Graylog.Core.Transport;
using Serilog.Templates;

namespace Krypton.Budgets.Logging.Hosting;

public static class LoggingCompositionStatic
{
    private static readonly NameResolver FUNCTIONS = new StaticMemberNameResolver(typeof(LogFormatterFunctionsStatic));

    public static IServiceCollection AddBudgetsLogging(this IServiceCollection services, IConfiguration config)
    {
        var logConf = new LoggerConfiguration().MinimumLevel.Verbose();

        foreach (var section in config.GetChildren())
        {
            switch (section.Key)
            {
                case "Levels":
                    logConf = SetupLogLevels(logConf, section);
                    break;

                case "Console":
                    logConf = SetupConsoleSink(logConf, section);
                    break;

                case "File":
                    logConf = SetupFileSinks(logConf, section);
                    break;

                case "Gelf":
                    logConf = SetupGelfSinks(logConf, section);
                    break;

                case "Debug":
                    logConf = SetupDebugSink(logConf, section);
                    break;
            }
        }

        return services.
            RemoveAll<ILoggerFactory>().
            AddSingleton<ILoggerFactory>(new SerilogLoggerFactory(logConf.CreateLogger()));
    }

    private static LoggerConfiguration SetupLogLevels(LoggerConfiguration logConf, IConfiguration config)
    {
        foreach (var kv in config.AsEnumerable(true))
        {
            var level = Enum.TryParse(kv.Value, out LogEventLevel lv) ? lv : LogEventLevel.Information;

            if (kv.Key == "_")
            {
                logConf = logConf.MinimumLevel.Is(level);
            }
            else
            {
                logConf = logConf.MinimumLevel.Override(kv.Key, level);
            }
        }

        return logConf;
    }

    private static LoggerConfiguration SetupConsoleSink(LoggerConfiguration logConf, IConfiguration config)
    {
        var options = config.Get<ConsoleSinkOptions>() ?? new();
        return logConf.WriteTo.Console(
            restrictedToMinimumLevel: options.Level,
            standardErrorFromLevel: options.StdErrorRedirect
        );
    }

    private static LoggerConfiguration SetupFileSinks(LoggerConfiguration logConf, IConfiguration config)
    {
        var sinks = config.Get<List<FileSinkOptions>>() ?? new();
        if (!sinks.Any())
        {
            return SetupFileSink(logConf, config.Get<FileSinkOptions>() ?? new());
        }

        foreach (var sink in sinks)
        {
            logConf = SetupFileSink(logConf, sink);
        }

        return logConf;
    }

    private static LoggerConfiguration SetupFileSink(LoggerConfiguration logConf, FileSinkOptions options)
    {
        return logConf.WriteTo.Async(c => c.File(
            "logs/" + options.Prefix + ".log",
            restrictedToMinimumLevel: options.Level,
            fileSizeLimitBytes: options.SizeLimit < 0 ? null : options.SizeLimit,
            shared: true,
            flushToDiskInterval: options.FlushInterval,
            rollingInterval: options.Rolling,
            rollOnFileSizeLimit: true,
            retainedFileCountLimit: options.FileCountLimit,
            retainedFileTimeLimit: options.FileTimeLimit
        ), blockWhenFull: true);
    }

    private static LoggerConfiguration SetupGelfSinks(LoggerConfiguration logConf, IConfiguration config)
    {
        var sinks = config.Get<List<GelfSinkOptions>>() ?? new();
        if (!sinks.Any())
        {
            return SetupGelfSink(logConf, config.Get<GelfSinkOptions>() ?? new());
        }

        foreach (var sink in sinks)
        {
            logConf = SetupGelfSink(logConf, sink);
        }

        return logConf;
    }

    private static LoggerConfiguration SetupGelfSink(LoggerConfiguration logConf, GelfSinkOptions options)
    {
        return logConf.WriteTo.Async(c => c.Graylog(new GraylogSinkOptions
        {
            HostnameOrAddress = options.Host.Host,
            Port = options.Host.Port,
            TransportType = TransportType.Http,
            UseSsl = options.Host.Scheme == "https",
            MinimumLogEventLevel = options.Level,
            MessageGeneratorType = MessageIdGeneratorType.Md5,
            IncludeMessageTemplate = true,
            UsernameInHttp = options.Username,
            PasswordInHttp = options.Password
        }), blockWhenFull: true);
    }

    private static LoggerConfiguration SetupDebugSink(LoggerConfiguration logConf, IConfiguration config)
    {
        var options = config.Get<SinkOptions>() ?? new();
        return logConf.WriteTo.Debug(
            new ExpressionTemplate("{@t:yyyy-MM-dd HH-mm-ss} - {@l:u} - {#if SourceContext is not null}Source: {ShortenPath(SourceContext)} - {#end}{@m}, {@r:j}\n" +
                "{#if Scope is not null}\tScope: {#each s in Scope}{ElementAt(s, 0)}: {ElementAt(s, 1)}{#delimit} | {#end}\n{#end}" +
                "{#if @x is not null}\t{@x}\n{#end}",
                nameResolver: FUNCTIONS),
            restrictedToMinimumLevel: options.Level
        );
    }
}
