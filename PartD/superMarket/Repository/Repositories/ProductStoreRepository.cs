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
    public class ProductStoreRepository: IProductStoreRepository
    {
        private readonly IContext context;
        public ProductStoreRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<ProductStore> Add(ProductStore value)
        {
            await context.ProducstsStore.AddAsync(value);
            await context.Save();
            return value;
        }

        public async Task<ProductStore> Get(int id)
        {
            return await context.ProducstsStore.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<ProductStore>> GetAll()
        {
            return await context.ProducstsStore.ToListAsync();
        }

        public async Task<ProductStore> GetByName(string name)
        {
            return await context.ProducstsStore.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<ProductStore> UpdateQty(int id, int qty)
        {
            var p= await Get(id);
            p.StockQty = qty;
            await context.Save();
            return p;
        }
    }
}
