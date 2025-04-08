using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using superMarket.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace superMarket.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceUser service;
        public UserController(IServiceUser service)
        {
            this.service = service;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await service.GetAll();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            return await service.Get(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<User> Post([FromBody] User value)
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

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
