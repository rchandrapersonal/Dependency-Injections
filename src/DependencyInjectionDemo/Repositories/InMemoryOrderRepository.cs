using System.Collections.Concurrent;
using DependencyInjectionDemo.Models;

namespace DependencyInjectionDemo.Repositories;

/// <summary>
/// A simple in-memory <see cref="IOrderRepository"/>.
/// Registered as a singleton so the store survives across resolutions.
/// </summary>
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<int, Order> _orders = new();

    public void Save(Order order) => _orders[order.Id] = order;

    public Order? GetById(int id) => _orders.TryGetValue(id, out var order) ? order : null;
}
