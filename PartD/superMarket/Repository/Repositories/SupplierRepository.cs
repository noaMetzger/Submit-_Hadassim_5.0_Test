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
    public class SupplierRepository : IRepository<Supplier>
    {
        private readonly IContext context;
        public SupplierRepository(IContext context)
        {
            this.context = context;
        }
        public async Task<Supplier> Add(Supplier value)
        {
            await context.Suppliers.AddAsync(value);
            await context.Save();
            return value;
        }

        public async Task<Supplier> Get(int id)
        {
            return await context.Suppliers.Include(sup=>sup.Orders).Include(sup=>sup.Stocks).FirstOrDefaultAsync(sup=>sup.Id == id);
        }
        //כל הספקים עם כל הסחורות
        public async Task<List<Supplier>> GetAll()
        {
            return await context.Suppliers.Include(sup => sup.Stocks).ToListAsync();
        }
    }
}
