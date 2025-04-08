using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> Get(int id);
        Task<T> Add(T value);
        Task<List<T>> GetAll();
    }
}
