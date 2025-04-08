using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IContext
    {
         public DbSet<Stock> Stocks { get; set; }
         public DbSet<Order> Orders { get; set; }
         public DbSet<ProductStore> ProducstsStore { get; set; }
         public DbSet<ProductOrder> ProducstsOrder { get; set; }
         public DbSet<Supplier> Suppliers { get; set; }
         public DbSet<User> Users { get; set; }

         public Task Save();

    }
}
