using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class ProductOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int StockId { get; set; }
        [ForeignKey("StockId")]
        public virtual Stock ? Stock { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order ? Order {  get; set; }

    }
}
