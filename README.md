# Dependency Injection Demo (.NET 8)

A small console application demonstrating the **Dependency Injection (DI)** pattern
with `Microsoft.Extensions.DependencyInjection`, plus a full unit/integration
test suite using **xUnit** and **Moq**.

## What it shows

- **Constructor injection** — `OrderService` receives all collaborators
  (`IOrderRepository`, `IMessageService`, `ILogger<T>`) through its constructor.
- **Programming to interfaces** — consumers depend on abstractions, so
  implementations are swappable (`EmailMessageService` ⇄ `SmsMessageService`)
  and mockable in tests.
- **Composition root** — everything is registered once in `Program.cs`.
- **Service lifetimes** — singleton (`IOrderRepository`) vs transient
  (`IMessageService`, `IOrderService`).

## Project layout

```
DependencyInjectionDemo.sln
├── src/DependencyInjectionDemo/        # console app
│   ├── Models/Order.cs
│   ├── Repositories/                   # IOrderRepository + in-memory impl
│   ├── Services/                       # IMessageService, OrderService, etc.
│   └── Program.cs                      # DI composition root
└── tests/DependencyInjectionDemo.Tests/
    ├── OrderServiceTests.cs            # unit tests with Moq mocks
    └── DependencyInjectionWiringTests.cs  # container/lifetime tests
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  (verify with `dotnet --version`)

## Run

```bash
dotnet run --project src/DependencyInjectionDemo
```

## Test

```bash
dotnet test
```

NuGet packages are restored automatically on the first `run`/`test`.
