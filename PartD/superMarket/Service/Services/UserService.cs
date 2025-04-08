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
    public class UserService : IServiceUser
    {
        private readonly IRepository<User> repository;
        private readonly IConfiguration config;
        public UserService(IRepository<User> repository, IConfiguration config)
        {
            this.repository = repository;
            this.config = config;
        }
        public async Task<User> Add(User value)
        {
            return await repository.Add(value);
        }

        public async Task<User> Get(int id)
        {
            return await repository.Get(id);
        }
        public async Task<List<User>> GetAll()
        {
            return await repository.GetAll();
        }
        public async Task<User> GetByEmailPass(string email, string pass)
        {
            return (await GetAll()).FirstOrDefault(x => x.Password == pass && x.Email == email);
        }
        public string Generate(User user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
            new Claim("name",user.Name),
            new Claim("email",user.Email),
            new Claim("role",user.Role),
            new Claim("id", user.Id.ToString()), 
            new Claim("password", user.Password) 
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"], config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
