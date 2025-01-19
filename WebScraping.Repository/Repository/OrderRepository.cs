using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraping.Core.Models;
using WebScraping.Core.Repositories;

namespace WebScraping.Repository.Repository
{
    public class OrderRepository : IOrderRepository
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Order>();
        }

        public async Task AddAsync(Order order)
        {
            await _dbSet.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(Order order)
        {
            _dbSet.Remove(order);
        }

        public async Task Remove(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("entity");

            _dbSet.Remove(entity);
        }

        public async Task<Order> Update(Order order)
        {
            _dbSet.Update(order);
            var updatedOrder = await GetByIdAsync(order.Id);
            return updatedOrder;
        }

    }
}
