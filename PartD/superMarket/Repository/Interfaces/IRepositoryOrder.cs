using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
namespace Repository.Interfaces
{
    public interface IRepositoryOrder:IRepository<Order>
    {
        Task<Order> UpdateStatus(int id, string status);
        Task<List<Order>> GetOrdersByUserId(int id);
        Task<List<Order>> GetOrdersBySupplierId(int id);
    }
}
