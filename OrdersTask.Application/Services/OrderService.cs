using Microsoft.Extensions.Logging;
using OrdersTask.Application.DTOs;
using OrdersTask.Application.Interfaces;
using OrdersTask.Domain.Entities;
using OrdersTask.Domain.Exceptions;
using OrdersTask.Domain.Interfaces;


namespace OrdersTask.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly ICacheService _cacheService;
        private const string CacheKeyPrefix = "order:";
        private static readonly TimeSpan CacheTTL = TimeSpan.FromMinutes(5);

        public OrderService(IOrderRepository orderRepository, ICacheService cacheService, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto)
        {
            _logger.LogInformation("Creating new order for customer: {CustomerName}", orderDto.CustomerName);

            var order = new Order
            {
                CustomerName = orderDto.CustomerName,
                Product = orderDto.Product,
                Amount = orderDto.Amount
            };

            var createdOrder = await _orderRepository.AddAsync(order);
            _logger.LogInformation("Order {OrderId} created successfully", createdOrder.Id);

            return MapToDto(createdOrder);
        }

        public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
        {
            var isExist = await _orderRepository.ExistsAsync(orderId);
            if(!isExist)
            {
                _logger.LogWarning("Attempted to delete non-existent order {OrderId}", orderId);
                throw new NotFoundException($"Order with ID {orderId} not found");
            }

            await _orderRepository.DeleteAsync(orderId);

            var cacheKey = $"{CacheKeyPrefix}{orderId}";
            await _cacheService.RemoveAsync(cacheKey);

            _logger.LogInformation("Order {OrderId} deleted successfully", orderId);
            return true;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            _logger.LogInformation("Retrieving all orders");
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToDto);
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            var cacheKey = $"{CacheKeyPrefix}{orderId}";

            var cachedOrder = await _cacheService.GetAsync<OrderDto>(cacheKey);
            if(cachedOrder != null)
            {
                _logger.LogInformation("Order {OrderId} retrieved from cache", orderId);
                return cachedOrder;
            }

            var order = await _orderRepository.GetByIdAsync(orderId);

            if(order == null)
            {
                _logger.LogWarning("Order {OrderId} not found", orderId);
                throw new NotFoundException($"Order with ID {orderId} not found");
            }

            var orderDto = MapToDto(order);

            await _cacheService.SetAsync(cacheKey, order, CacheTTL);
            _logger.LogInformation("Order {OrderId} cached for {TTL} minutes", orderId, CacheTTL.TotalMinutes);

            return orderDto;
        }


        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.Id,
                CustomerName = order.CustomerName,
                Product = order.Product,
                Amount = order.Amount,
                CreatedAt = order.CreatedAt
            };
        }
    }
}
