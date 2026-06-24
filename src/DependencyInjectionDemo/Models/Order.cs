namespace DependencyInjectionDemo.Models;

/// <summary>
/// A simple domain model representing a customer order.
/// </summary>
public class Order
{
    public int Id { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
