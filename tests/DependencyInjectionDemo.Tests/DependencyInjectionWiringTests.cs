using DependencyInjectionDemo.Repositories;
using DependencyInjectionDemo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace DependencyInjectionDemo.Tests;

/// <summary>
/// Verifies the DI container resolves the full object graph and honours
/// the registered service lifetimes.
/// </summary>
public class DependencyInjectionWiringTests
{
    private static ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddTransient<IMessageService, EmailMessageService>();
        services.AddTransient<IOrderService, OrderService>();
        return services.BuildServiceProvider();
    }

    [Fact]
    public void Container_ResolvesOrderService_WithRealDependencies()
    {
        using var provider = BuildProvider();

        var service = provider.GetRequiredService<IOrderService>();

        Assert.IsType<OrderService>(service);
    }

    [Fact]
    public void Repository_IsSingleton_SameInstanceEachTime()
    {
        using var provider = BuildProvider();

        var first = provider.GetRequiredService<IOrderRepository>();
        var second = provider.GetRequiredService<IOrderRepository>();

        Assert.Same(first, second);
    }

    [Fact]
    public void MessageService_IsTransient_NewInstanceEachTime()
    {
        using var provider = BuildProvider();

        var first = provider.GetRequiredService<IMessageService>();
        var second = provider.GetRequiredService<IMessageService>();

        Assert.NotSame(first, second);
    }

    [Fact]
    public void Container_PersistsOrder_AcrossResolutions_ViaSingletonRepository()
    {
        using var provider = BuildProvider();

        var orderService = provider.GetRequiredService<IOrderService>();
        var placed = orderService.PlaceOrder("wiring@example.com", 99m);

        // A separately-resolved repository sees the same singleton store.
        var repository = provider.GetRequiredService<IOrderRepository>();
        var fetched = repository.GetById(placed.Id);

        Assert.NotNull(fetched);
        Assert.Equal("wiring@example.com", fetched!.CustomerEmail);
    }
}
