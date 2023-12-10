using Microsoft.Extensions.Logging;

namespace Krypton.Budgets.Tests.Utils;

public class ProxyLogger<T> : ILogger<T>
{
    private readonly ILogger proxy;

    public ProxyLogger(ILogger proxy) { this.proxy = proxy; }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
        proxy.BeginScope(state);

    public bool IsEnabled(LogLevel logLevel) => proxy.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => proxy.Log(logLevel, eventId, state, exception, formatter);
}
