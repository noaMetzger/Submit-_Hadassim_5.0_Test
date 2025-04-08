using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;

namespace Service.Interfaces
{
    public interface IServiceSupplier:IService<Supplier>
    {
        Task<Supplier> GetByEmailPass(string email, string pass);
        string Generate(Supplier user);
    }
}
