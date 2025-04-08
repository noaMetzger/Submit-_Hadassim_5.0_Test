using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Repository.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace superMarket.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IServiceOrder service;
        public OrderController(IServiceOrder service)
        {
            this.service = service;
        }
        [Authorize(Roles = "grocer")]
        [HttpGet("orderuser")]
        public async Task<IActionResult> GetOrdersUser()
        {

            var idClaim = User.FindFirst("id");

            if (idClaim == null || string.IsNullOrEmpty(idClaim.Value))
            {
                return BadRequest("User ID claim is missing or invalid.");
            }

            if (!int.TryParse(idClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing or invalid.");
            }

            var filteredOrders = await service.GetOrdersByUserId(userId);

            return Ok(filteredOrders);
        }

        // GET: api/<OrderController>
        [Authorize(Roles = "supplier")]
        [HttpGet("ordersupplier")]
        public async Task<IActionResult> GetOrdersSupplier()
        {
            var idClaim = User.FindFirst("id");

            if (idClaim == null || string.IsNullOrEmpty(idClaim.Value))
            {
                return BadRequest("User ID claim is missing or invalid.");
            }

            if (!int.TryParse(idClaim.Value, out var supplierId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var token = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing or invalid.");
            }

            var filteredOrders = await service.GetOrdersBySupplierId(supplierId);

            return Ok(filteredOrders);
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<Order> Get(int id)
        {
            return await service.Get(id);
        }

        // POST api/<OrderController>
        [Authorize(Roles = "grocer")]
        [HttpPost]
        public async Task<Order> Post([FromBody] Order value)
        {
            return await service.Add(value);
        }

        // PUT api/<OrderController>/5
        [Authorize(Roles = "grocer,supplier")]
        [HttpPut("{id}")]
        public async Task<Order> Put(int id, string status)
        {
            var order = await service.UpdateStatus(id, status);

            return order;
        }


        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
