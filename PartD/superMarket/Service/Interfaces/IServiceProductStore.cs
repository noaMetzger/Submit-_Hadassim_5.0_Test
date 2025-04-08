using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;

namespace Service.Interfaces
{
    public interface IServiceProductStore:IService<ProductStore>
    {
        Task<ProductStore> UpdateQty(int id, int qty);
    }
}
