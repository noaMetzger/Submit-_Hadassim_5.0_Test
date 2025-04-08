using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Mock
{
    public class DataBase : DbContext, IContext
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductStore> ProducstsStore { get; set; }
        public DbSet<ProductOrder> ProducstsOrder { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public async Task Save()
        {
            await SaveChangesAsync();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=NOA;database=SuperMarket;trusted_connection=true;TrustServerCertificate=true");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ProductOrder>()
        //        .HasOne(po => po.Stock)
        //        .WithMany()
        //        .HasForeignKey(po => po.StockId)
        //        .OnDelete(DeleteBehavior.Restrict); // מניעת מחיקת cascade עבור Stock

        //    modelBuilder.Entity<ProductOrder>()
        //        .HasOne(po => po.Order)
        //        .WithMany(o => o.Products)
        //        .HasForeignKey(po => po.OrderId)
        //        .OnDelete(DeleteBehavior.Cascade); // מחיקת cascade עבור Order
        //}
    }
}
