using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace superMarket.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStoreController : ControllerBase
    {
        private readonly IServiceProductStore service;
        public ProductStoreController(IServiceProductStore service)
        {
            this.service = service;
        }
        // GET: api/<ProductStoreController>
        [HttpGet]
        public async Task<List<ProductStore>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<ProductStoreController>/5
        [HttpGet("{id}")]
        public async Task<ProductStore> Get(int id)
        {
            return await service.Get(id);
        }

        // POST api/<ProductStoreController>
        [HttpPost]
        public async Task<ProductStore> Post([FromBody] ProductStore value)
        {
            return await service.Add(value);
        }

        // PUT api/<ProductStoreController>/5
        [HttpPut("{id}")]
        public async Task<ProductStore> Put(int id, int value)
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            var role = roleClaim?.Value;

            if (string.IsNullOrEmpty(role))
            {
                return null;
            }

            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var normalizedRole = role.ToLower();
            if (normalizedRole != "grocer")
            {
                return null;
            }

            return await service.UpdateQty(id, value);
        }

        // DELETE api/<ProductStoreController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
