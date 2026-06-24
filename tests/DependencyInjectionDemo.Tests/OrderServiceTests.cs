using DependencyInjectionDemo.Models;
using DependencyInjectionDemo.Repositories;
using DependencyInjectionDemo.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DependencyInjectionDemo.Tests;

/// <summary>
/// Unit tests for <see cref="OrderService"/>. Because the service depends on
/// interfaces, we hand it Moq-generated fakes and assert how it interacts
/// with them — no real repository, logger, or message channel involved.
/// </summary>
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repository = new();
    private readonly Mock<IMessageService> _messageService = new();
    private readonly Mock<ILogger<OrderService>> _logger = new();

    private OrderService CreateSut() =>
        new(_repository.Object, _messageService.Object, _logger.Object);

    [Fact]
    public void PlaceOrder_PersistsTheOrder()
    {
        var sut = CreateSut();

        var order = sut.PlaceOrder("customer@example.com", 25m);

        Assert.Equal("customer@example.com", order.CustomerEmail);
        Assert.Equal(25m, order.Amount);
        _repository.Verify(r => r.Save(It.Is<Order>(o => o.Amount == 25m)), Times.Once);
    }

    [Fact]
    public void PlaceOrder_SendsConfirmationMessageToCustomer()
    {
        var sut = CreateSut();

        sut.PlaceOrder("customer@example.com", 10m);

        _messageService.Verify(
            m => m.Send("customer@example.com", It.Is<string>(msg => msg.Contains("has been received"))),
            Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void PlaceOrder_WithMissingEmail_Throws(string? email)
    {
        var sut = CreateSut();

        Assert.Throws<ArgumentException>(() => sut.PlaceOrder(email!, 10m));
        _repository.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void PlaceOrder_WithNonPositiveAmount_Throws(decimal amount)
    {
        var sut = CreateSut();

        Assert.Throws<ArgumentOutOfRangeException>(() => sut.PlaceOrder("a@b.com", amount));
        _messageService.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
