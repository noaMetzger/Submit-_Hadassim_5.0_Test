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
    public class ProductOrderRepository: IRepository<ProductOrder>
    {
        private readonly IContext context;
        public ProductOrderRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<ProductOrder> Add(ProductOrder value)
        {
            await context.ProducstsOrder.AddAsync(value);
            await context.Save();
            return value;
        }

        public async Task<ProductOrder> Get(int id)
        {
            return await context.ProducstsOrder.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<ProductOrder>> GetAll()
        {
            return await context.ProducstsOrder.ToListAsync();
        }
    }
}
