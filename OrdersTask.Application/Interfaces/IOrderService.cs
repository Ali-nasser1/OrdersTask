using OrdersTask.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersTask.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto);
        Task<bool> DeleteOrderByIdAsync(Guid orderId);
    }
}
