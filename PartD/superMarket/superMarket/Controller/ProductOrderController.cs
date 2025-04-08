using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace superMarket.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOrderController : ControllerBase
    {
        private readonly IService<ProductOrder> service;
        public ProductOrderController(IService<ProductOrder> service)
        {
            this.service = service;
        }
        // GET: api/<ProductOrderController>
        [HttpGet]
        public async Task<List<ProductOrder>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<ProductOrderController>/5
        [HttpGet("{id}")]
        public async Task<ProductOrder> Get(int id)
        {
            return await service.Get(id);
        }

        // POST api/<ProductOrderController>
        [HttpPost]
        public async Task<ProductOrder> Post([FromBody] ProductOrder value)
        {
            return await service.Add(value);
        }

        // PUT api/<ProductOrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductOrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
