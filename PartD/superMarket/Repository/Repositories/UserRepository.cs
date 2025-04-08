using Repository.Interfaces;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IContext context;
        public UserRepository(IContext context)
        {
            this.context = context;
        }
        public async Task<User> Add(User value)
        {
            await context.Users.AddAsync(value);
            await context.Save();
            return value;
        }

        public async Task<User> Get(int id)
        {
            return await context.Users.Include(x=>x.Orders).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }
    }
}
