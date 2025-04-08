using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Interfaces
{
    public interface IProductStoreRepository:IRepository<ProductStore>
    {
        public Task<ProductStore> UpdateQty(int id, int qty);
        public Task<ProductStore> GetByName(string name);
    }
}
