using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class OrderRepository: IRepositoryOrder
    {
        private readonly IContext context;
        public OrderRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<Order> Add(Order value)
        {
            await context.Orders.AddAsync(value);
            await context.Save();
            return value;
        }

        public async Task<Order> Get(int id)
        {
            return await context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetAll()
        {
            return await context.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetOrdersBySupplierId(int id)
        {
            return await context.Orders.Include(x => x.Products).Where(x => x.SupplierId == id).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByUserId(int id)
        {
            return await context.Orders.Include(x => x.Products).Where(x=>x.UserId==id).ToListAsync();
        }

        public async Task<Order> UpdateStatus(int id, string status)
        {
            var order = await Get(id);
            order.Status = status;
            await context.Save();
            return order;
        }
    }
}
