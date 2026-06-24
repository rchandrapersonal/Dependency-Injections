using DependencyInjectionDemo.Repositories;
using DependencyInjectionDemo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ---------------------------------------------------------------------------
// Composition root: register every dependency with the DI container exactly
// once, here. Nothing else in the app uses "new" for these services.
// ---------------------------------------------------------------------------
var services = new ServiceCollection();

services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));

// Lifetimes:
//   Singleton  -> one instance for the whole app (good for the in-memory store).
//   Transient  -> a fresh instance every time it is requested.
services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

// Swap this single line to SmsMessageService to change the channel everywhere.
services.AddTransient<IMessageService, EmailMessageService>();

services.AddTransient<IOrderService, OrderService>();

using var provider = services.BuildServiceProvider();

// Resolve the top-level service; the container builds the whole dependency graph.
var orderService = provider.GetRequiredService<IOrderService>();

var order = orderService.PlaceOrder("customer@example.com", 49.99m);

Console.WriteLine($"Placed order #{order.Id} for {order.Amount:C}.");
