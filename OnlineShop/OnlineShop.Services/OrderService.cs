using OnlineShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Core.Models;
using OnlineShop.Core.ViewModels;

namespace OnlineShop.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> OrderContext;
        public OrderService(IRepository<Order> OrderContext)
        {
            this.OrderContext = OrderContext;
        }

        public void CreateOrder(Order baseOrder, List<BasketItemVM> basketItems)
        {
            foreach (var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItem {

                    ProductId = item.BasketItemId,
                    ProductName = item.ProductName,
                    ProductImage = item.Image,
                    price = item.Price,
                    Quantity =item.Quantity
                });
            }

            OrderContext.Insert(baseOrder);
            OrderContext.Commit();
        }
    }
}
