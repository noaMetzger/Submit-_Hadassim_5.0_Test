using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public static class ExtensionRepository
    {
        public static IServiceCollection AddRepoitory(this IServiceCollection services)
        {
            services.AddScoped<IRepository<ProductOrder>, ProductOrderRepository>();
            services.AddScoped<IProductStoreRepository, ProductStoreRepository>();
            services.AddScoped<IRepository<Stock>, StockRepository>();
            services.AddScoped<IRepository<Supplier>, SupplierRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepositoryOrder, OrderRepository>();
            return services;
        }
    }
}
