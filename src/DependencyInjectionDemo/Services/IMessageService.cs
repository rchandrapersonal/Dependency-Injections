namespace DependencyInjectionDemo.Services;

/// <summary>
/// Abstraction for sending a message to a recipient.
/// Consumers depend on this interface, not a concrete email/SMS class,
/// so the implementation can be swapped or mocked freely.
/// </summary>
public interface IMessageService
{
    void Send(string recipient, string message);
}
