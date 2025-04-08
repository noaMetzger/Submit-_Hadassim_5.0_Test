using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
using Service.Interfaces;
using Service.Models;

namespace Service.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IRepository<Stock> stockRepository;
        private readonly IRepositoryOrder orderRepository;
        private readonly IProductStoreRepository productRepository;
        private readonly IRepository<ProductOrder> productOrderRepository;

        public PurchaseService(IRepository<Stock> stockRepository, IRepositoryOrder orderRepository, IProductStoreRepository productRepository, IRepository<ProductOrder> productOrderRepository)
        {
            this.stockRepository = stockRepository;
            this.productRepository = productRepository;
            this.orderRepository = orderRepository;
            this.productOrderRepository = productOrderRepository;
        }
        public async Task Purchase(List<ProductInPurchase> products)
        {
            List<Stock> stocks = await stockRepository.GetAll();
            var productStore = await productRepository.GetAll();

            var supplierOrders = new Dictionary<int, List<(string ProductName, int Quantity, int StockId, double Price)>>();

            foreach (var product in products)
            {
                var p = productStore.FirstOrDefault(x => x.Name == product.Name);
                if (p == null) continue;

                p = await productRepository.UpdateQty(p.Id, p.StockQty - product.Qty);

                if (p.MinCount <= p.StockQty) continue;

                var stockName = stocks.Where(x => x.Name == product.Name).ToList();
                var cheapest = stockName.OrderBy(x => x.Price).FirstOrDefault();

                if (cheapest == null) continue;

                // הוספת המוצר למילון לפי מזהה הספק
                if (!supplierOrders.ContainsKey(cheapest.SupplierId))
                {
                    supplierOrders[cheapest.SupplierId] = new List<(string ProductName, int Quantity, int StockId, double Price)>();
                }
                int amount=Math.Max(cheapest.MinAmount, (int)(p.MinCount * 1.5 - p.StockQty)); // חישוב כמות ההזמנה 
                p = await productRepository.UpdateQty(p.Id, p.StockQty + amount); // עדכון כמות המוצר במלאי
                supplierOrders[cheapest.SupplierId].Add((product.Name, amount, cheapest.Id, cheapest.Price));
            }

            // עובר על הספקים ומבצע הזמנות
            foreach (var supplier in supplierOrders)
            {
                var productsForSupplier = supplier.Value; // רשימת המוצרים 
                                                         
                                                         
                var totalPrice = productsForSupplier.Sum(product => product.Price * product.Quantity); // חישוב המחיר הכולל

                var order = new Order
                {
                    SupplierId = supplier.Key,
                    Date = DateTime.Now,
                    Status="בעיבוד",
                    UserId=1,
                    Total=totalPrice   
                };

                var ord= await orderRepository.Add(order);

                foreach (var p1 in productsForSupplier) {
                    var newProduct = new ProductOrder
                    {
                        Name = p1.ProductName,
                        Quantity = p1.Quantity,
                        StockId = p1.StockId,
                        OrderId = ord.Id
                    };
                    await productOrderRepository.Add(newProduct);
                }
                
            }

        }
    }
}
