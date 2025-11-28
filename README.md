# Orders API - Technical Assessment

A RESTful API built with .NET 8, Entity Framework Core, and Redis for managing orders with caching capabilities.

##  Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
OrdersAPI/
├── OrdersAPI.Domain/          # Core business entities and interfaces (no dependencies)
├── OrdersAPI.Application/     # Business logic, DTOs, and services
├── OrdersAPI.Infrastructure/  # External concerns (Database, Redis)
└── OrdersAPI.API/            # Presentation layer (Controllers, Middleware)
```

### Dependency Flow
```
API → Application → Domain
API → Infrastructure → Domain
Infrastructure → Domain
```

The Domain layer is at the center with no external dependencies, ensuring the business logic remains independent of frameworks and tools.

##  Features

- ✅ RESTful API with full CRUD operations
- ✅ Redis caching with 5-minute TTL
- ✅ Entity Framework Core with SQL Server
- ✅ Clean Architecture pattern
- ✅ Repository Pattern
- ✅ Dependency Injection
- ✅ FluentValidation for input validation
- ✅ Global exception handling middleware
- ✅ Structured logging with Serilog

##  Prerequisites

- .NET 8 SDK
- SQL Server (or Docker)
- Redis (or Docker)


##  API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/orders` | Get all orders |
| GET | `/api/orders/{id}` | Get order by ID (with caching) |
| POST | `/api/orders` | Create a new order |
| DELETE | `/api/orders/{id}` | Delete an order |
| GET | `/health` | Health check endpoint |


### Example Response
```json
{
  "success": true,
  "message": "Order created successfully",
  "data": {
    "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "customerName": "John Doe",
    "product": "Laptop",
    "amount": 1299.99,
    "createdAt": "2024-01-15T10:30:00Z"
  }
}
```

##  Project Highlights

- ✅ **Clean Architecture**: Clear separation of concerns
- ✅ **SOLID Principles**: Single Responsibility
- ✅ **Async/Await**: All I/O operations are asynchronous
- ✅ **Error Handling**: Global exception middleware with proper HTTP status codes
- ✅ **Logging**: Structured logging with Serilog
- ✅ **Validation**: FluentValidation for request validation

