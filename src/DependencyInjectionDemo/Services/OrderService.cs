using DependencyInjectionDemo.Models;
using DependencyInjectionDemo.Repositories;
using Microsoft.Extensions.Logging;

namespace DependencyInjectionDemo.Services;

public interface IOrderService
{
    Order PlaceOrder(string customerEmail, decimal amount);
}

/// <summary>
/// Business logic for placing an order. It depends only on abstractions
/// (<see cref="IOrderRepository"/>, <see cref="IMessageService"/>), all
/// supplied via the constructor. This is what makes the class unit-testable:
/// the test can pass in mocks instead of real infrastructure.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMessageService _messageService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository repository,
        IMessageService messageService,
        ILogger<OrderService> logger)
    {
        _repository = repository;
        _messageService = messageService;
        _logger = logger;
    }

    public Order PlaceOrder(string customerEmail, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new ArgumentException("Customer email is required.", nameof(customerEmail));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");

        var order = new Order
        {
            Id = Random.Shared.Next(1, int.MaxValue),
            CustomerEmail = customerEmail,
            Amount = amount
        };

        _repository.Save(order);
        _logger.LogInformation("Order {OrderId} saved for {Email}.", order.Id, customerEmail);

        _messageService.Send(customerEmail, $"Your order #{order.Id} for {amount:C} has been received.");

        return order;
    }
}
