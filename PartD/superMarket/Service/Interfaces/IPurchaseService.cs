using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;

namespace Service.Interfaces
{
    public interface IPurchaseService
    {
        Task Purchase(List<ProductInPurchase> product);
    }
}
