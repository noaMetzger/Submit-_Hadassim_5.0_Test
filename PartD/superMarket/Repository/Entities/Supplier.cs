using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string CompanyName { get; set; } 
        public string AgentName {  get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Stock> ? Stocks { get; set; }
        public virtual ICollection<Order> ? Orders { get; set; }
    }
}
