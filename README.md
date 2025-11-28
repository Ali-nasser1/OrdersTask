# Orders API - Technical Assessment

A RESTful API built with .NET 8, Entity Framework Core, and Redis for managing orders with caching capabilities.

##  Short Questions Answers:
1. Redis vs SQL – key differences.

**Redis:**
- In-memory key-value store
- Extremely fast
- Data is temporary unless persistence is enabled
- Best for caching, sessions, and real-time counters

**SQL:**
- Disk-based relational database
- uses structured tables and relationships
- Best for long-term and raliable data storage

2. When not to use caching? 
- Data is frequently changed
- if we need real-time accuracy
- Huge data and limited memory

3. What if Redis is down? 
- The system should instantly retrieve the data from SQL
- No Downtime will occur, but we will find a delay in response
- Log the error
- make a retry or fallback mechanism

4. Optimistic vs pessimistic locking? 

**Optimistic locking:**
- Allow reading and writing without a lock
- Validate before saving data if it has changed
- suitable for systems with low writes and high reads

**pessimistic lock:**
- Lock the row before Editing
- prevent other users from editing
- suitable for high conflicts

5. Ways to scale a .NET backend? 
- Horizontal scaling: Add more server instances behind a load balancer
- Vertical scaling: Increase CPU, RAM, disk on existing servers
- Database scaling: read replicas, sharding
- caching using Redis
- Microservices Architecture: Break monolith into smaller services
- Queue-based processing: RabbitMQ, Kafka

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

