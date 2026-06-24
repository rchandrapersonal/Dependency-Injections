using Microsoft.Extensions.Logging;

namespace DependencyInjectionDemo.Services;

/// <summary>
/// An alternative <see cref="IMessageService"/> implementation.
/// Demonstrates that the consumer can be wired to a different
/// implementation without any code change in the consumer itself.
/// </summary>
public class SmsMessageService : IMessageService
{
    private readonly ILogger<SmsMessageService> _logger;

    public SmsMessageService(ILogger<SmsMessageService> logger)
    {
        _logger = logger;
    }

    public void Send(string recipient, string message)
    {
        _logger.LogInformation("SMS to {Recipient}: {Message}", recipient, message);
    }
}
