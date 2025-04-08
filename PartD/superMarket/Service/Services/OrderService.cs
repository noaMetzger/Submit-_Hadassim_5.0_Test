using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Interfaces;
using Repository.Entities;
using Repository.Interfaces;

namespace Service.Services
{
    public class OrderService : IServiceOrder
    {
        private readonly IRepositoryOrder repository;
        public OrderService(IRepositoryOrder repository)
        {
            this.repository = repository;
        }
        public async Task<Order> Add(Order value)
        {
            return await repository.Add(value);
        }

        public async Task<Order> Get(int id)
        {
            return await repository.Get(id);  
        }

        public async Task<List<Order>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<List<Order>> GetOrdersBySupplierId(int id)
        {
            return await repository.GetOrdersBySupplierId(id);
         }

        public async Task<List<Order>> GetOrdersByUserId(int id)
        {
            return await repository.GetOrdersByUserId(id);
        }

        public async Task<Order> UpdateStatus(int id, string status)
        {
            return await repository.UpdateStatus(id, status);
        }
    }
}
