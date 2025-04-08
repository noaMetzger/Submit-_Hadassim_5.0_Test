using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using superMarket.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace superMarket.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IServiceSupplier service;
        public SupplierController(IServiceSupplier service)
        {
            this.service = service;
        }
        // GET: api/<SupplierController>
        [HttpGet]
        public async Task<List<Supplier>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<SupplierController>/5
        [HttpGet("{id}")]
        public async Task<Supplier> Get(int id)
        {
            return await service.Get(id);
        }

        // POST api/<SupplierController>
        [HttpPost]
        public async Task<Supplier> Post([FromBody] Supplier value)
        {
            return await service.Add(value);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Post(UserLogin userLogin)
        {
            var user = (await service.GetByEmailPass(userLogin.Email, userLogin.Password));
            if (user != null)
            {
                var token = service.Generate(user);
                return Ok(token);
            }
            return NotFound("user not found");
        }

        // PUT api/<SupplierController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SupplierController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
