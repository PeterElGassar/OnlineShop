using OnlineShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineShop.Core.Contracts
{
    public interface IBasketService
    {

        void AddToBasket(HttpContextBase httpContext, string productId);
        void RemoveFromBasket(HttpContextBase httpContext, string itemId);

        List<BasketItemVM> GetBasketItems(HttpContextBase httpContext);
        BasketSummaryVM GetBasketSummary(HttpContextBase httpContext);

        void ClearBasketItems(HttpContextBase httpContext);
    }
}
