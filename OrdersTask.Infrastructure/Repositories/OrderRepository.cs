using Microsoft.EntityFrameworkCore;
using OrdersTask.Domain.Entities;
using OrdersTask.Domain.Interfaces;
using OrdersTask.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersTask.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if(order != null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        => await _dbContext.Orders.AnyAsync(o => o.Id == id);

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbContext.Orders
                         .AsNoTracking()
                         .OrderByDescending(o => o.CreatedAt)
                         .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        => await _dbContext.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id ==  id);

        public async Task UpdateAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}
