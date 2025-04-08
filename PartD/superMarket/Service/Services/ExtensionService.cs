using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Repositories;

namespace Service.Services
{
    public static class ExtensionService
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddRepoitory();
            services.AddScoped<IServiceOrder, OrderService>();
            services.AddScoped<IService<ProductOrder>, ProductOrderService>();
            services.AddScoped<IServiceProductStore, ProductStoreService>();
            services.AddScoped<IService<Stock>, StockService >();
            services.AddScoped<IServiceUser, UserService >();
            services.AddScoped<IServiceSupplier, SupplierService >();
            services.AddScoped<IPurchaseService, PurchaseService >();
            return services;

        }
    }
}
