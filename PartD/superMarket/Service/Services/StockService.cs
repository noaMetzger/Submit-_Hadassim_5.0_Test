using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;
using Service.Interfaces;

namespace Service.Services
{
    public class StockService:IService<Stock>
    {
        private readonly IRepository<Stock> repository;
        public StockService(IRepository<Stock> repository)
        {
            this.repository = repository;
        }
        public async Task<Stock> Add(Stock value)
        {
            return await repository.Add(value);
        }

        public async Task<Stock> Get(int id)
        {
            return await repository.Get(id);
        }
        public async Task<List<Stock>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<Stock> GetCheapestStock(string name, int qty)
        {
            return (await GetAll()).Where(x=>x.Name==name&&x.MinAmount<=qty).OrderBy(x=>x.Price).FirstOrDefault();
        }

        public async Task<List<Stock>> GetStocksByName(string name)
        {
            return (await GetAll()).Where(x => x.Name == name).ToList();
        }

    }
}
