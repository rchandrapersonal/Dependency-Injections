using DependencyInjectionDemo.Models;

namespace DependencyInjectionDemo.Repositories;

/// <summary>
/// Abstraction over order persistence.
/// </summary>
public interface IOrderRepository
{
    void Save(Order order);
    Order? GetById(int id);
}
