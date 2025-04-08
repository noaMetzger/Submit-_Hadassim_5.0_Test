using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class ProductStore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StockQty { get; set; }
        public int MinCount { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User ? User { get; set; }
    }
}
