using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Service.Services
{
    public class SupplierService:IServiceSupplier
    {
        private readonly IRepository<Supplier> repository;
        private readonly IConfiguration config;
        public SupplierService(IRepository<Supplier> repository, IConfiguration config)
        {
            this.repository = repository;
            this.config = config;
        }
        public async Task<Supplier> Add(Supplier value)
        {
            return await repository.Add(value);
        }

        public async Task<Supplier> Get(int id)
        {
            return await repository.Get(id);
        }
        public async Task<List<Supplier>> GetAll()
        {
            return await repository.GetAll();
        }
        public async Task<Supplier> GetByEmailPass(string email, string pass)
        {
            return (await GetAll()).FirstOrDefault(x => x.Password == pass && x.Email == email);
        }
        public string Generate(Supplier supplier)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
            new Claim("agentName",supplier.AgentName),
            new Claim("companyName",supplier.CompanyName),
            new Claim("email",supplier.Email),
            new Claim("role",supplier.Role),
            new Claim("id", supplier.Id.ToString()),  // הוספת ה-id של המשתמש
            new Claim("password", supplier.Password) // הוספת הסיסמה, אם יש צורך בכך


            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
