using Microsoft.Extensions.Logging;

namespace DependencyInjectionDemo.Services;

/// <summary>
/// Concrete <see cref="IMessageService"/> that "sends" an email.
/// Its own dependency (the logger) is injected via the constructor.
/// </summary>
public class EmailMessageService : IMessageService
{
    private readonly ILogger<EmailMessageService> _logger;

    public EmailMessageService(ILogger<EmailMessageService> logger)
    {
        _logger = logger;
    }

    public void Send(string recipient, string message)
    {
        _logger.LogInformation("EMAIL to {Recipient}: {Message}", recipient, message);
    }
}
