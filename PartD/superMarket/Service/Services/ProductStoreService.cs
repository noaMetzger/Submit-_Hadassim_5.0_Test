using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ProductStoreService:IServiceProductStore
    {
        private readonly IProductStoreRepository repository;
        public ProductStoreService(IProductStoreRepository repository)
        {
            this.repository = repository;
        }
        public async Task<ProductStore> Add(ProductStore value)
        {
            return await repository.Add(value);
        }

        public async Task<ProductStore> Get(int id)
        {
            return await repository.Get(id);
        }
        public async Task<List<ProductStore>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<ProductStore> UpdateQty(int id, int qty)
        {
            return await repository.UpdateQty(id, qty);
        }
    }
}
