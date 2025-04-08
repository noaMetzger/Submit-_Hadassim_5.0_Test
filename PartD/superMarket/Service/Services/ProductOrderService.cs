using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Interfaces;

namespace Service.Services
{
    public class ProductOrderService:IService<ProductOrder>
    {
        private readonly IRepository<ProductOrder> repository;
        private readonly IProductStoreRepository productStore;

        public ProductOrderService(IRepository<ProductOrder> repository, IProductStoreRepository productStore)
        {
            this.repository = repository;
            this.productStore = productStore;
        }
        public async Task<ProductOrder> Add(ProductOrder value)
        {
            var prodOrder= await repository.Add(value);
            var prodStore = await productStore.GetByName(value.Name);
            if (prodStore != null) {
                await productStore.UpdateQty(prodStore.Id, prodStore.StockQty+value.Quantity);
            }
            else
            {
                prodStore = new ProductStore()
                {
                    Name = value.Name,
                    StockQty = value.Quantity,
                    MinCount = 0,
                    UserId = 1
                };
                await productStore.Add(prodStore);
            }
            return prodOrder;
        }

        public async Task<ProductOrder> Get(int id)
        {
            return await repository.Get(id);
        }
        public async Task<List<ProductOrder>> GetAll()
        {
            return await repository.GetAll();
        }
    }
}
