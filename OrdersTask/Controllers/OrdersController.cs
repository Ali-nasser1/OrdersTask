using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersTask.Application.DTOs;
using OrdersTask.Application.Interfaces;

namespace OrdersTask.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<CreateOrderDto> _validator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            IOrderService orderService,
            IValidator<CreateOrderDto> validator,
            ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _validator = validator;
            _logger = logger;
        }


        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            _logger.LogInformation($"Getting order {id}");
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(ApiResponse<OrderDto>.SuccessResponse(order, "Order retrieved successfully"));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrderDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAllOrders()
        {
            _logger.LogInformation("Getting All orders");
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(ApiResponse<IEnumerable<OrderDto>>.SuccessResponse(orders, "Orders retrieved successfully"));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            _logger.LogInformation($"Creating order for customer: {orderDto.CustomerName}");

            var validationResult = await _validator.ValidateAsync(orderDto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                                             .GroupBy(e => e.PropertyName)
                                             .ToDictionary(
                                              g => g.Key,
                                              g => g.Select(e => e.ErrorMessage).ToArray());

                return BadRequest(ApiResponse<Object>.FailureResponse("Validation failed", errors));
            }

            var order = await _orderService.CreateOrderAsync(orderDto);
            return CreatedAtAction(
                nameof(GetOrder),
                new { id = order.OrderId },
                ApiResponse<OrderDto>.SuccessResponse(order, "Order created successfully")
            );
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            _logger.LogInformation($"Deleting order {id}");
            var result = await _orderService.DeleteOrderByIdAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Order deleted successfully"));
        }
    }
}
