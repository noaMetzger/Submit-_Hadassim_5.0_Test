using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace superMarket.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IService<Stock> service;
        public StockController(IService<Stock> service)
        {
            this.service = service;
        }
        // GET: api/<StockController>
        [HttpGet]
        public async Task<List<Stock>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<StockController>/5
        [HttpGet("{id}")]
        public async Task<Stock> Get(int id)
        {
            return await service.Get(id);
        }

        // POST api/<StockController>
        [HttpPost]
        public async Task<Stock> Post([FromBody] Stock value)
        {
            return await service.Add(value);
        }

        // PUT api/<StockController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StockController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
