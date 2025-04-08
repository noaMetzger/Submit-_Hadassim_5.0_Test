using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Repository.Repositories
{
    public class StockRepository : IRepository<Stock>
    {
        private readonly IContext context;
        public StockRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<Stock> Add(Stock value)
        {
            await context.Stocks.AddAsync(value);
            await context.Save();
            return value;
        }

        public async Task<Stock> Get(int id)
        {
            return await context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Stock>> GetAll()
        {
            return await context.Stocks.ToListAsync();
        }
    }
}
