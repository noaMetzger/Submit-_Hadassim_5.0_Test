using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Status {get; set; }
        public double Total { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<ProductOrder> ? Products { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User ? User { get; set; }
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier ? Supplier { get; set; }


    }
}
