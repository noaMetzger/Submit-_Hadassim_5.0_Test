using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IServiceUser:IService<User>
    {
        Task<User> GetByEmailPass(string email, string pass);
        string Generate(User user);
    }
}
