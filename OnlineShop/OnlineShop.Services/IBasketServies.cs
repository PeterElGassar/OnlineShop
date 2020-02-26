using System.Collections.Generic;
using System.Web;
using OnlineShop.Core.ViewModels;

namespace OnlineShop.Services
{
    public interface IBasketServies
    {
        void AddToBasket(HttpContextBase httpContext, string productId);
        void RemoveFromBasket(HttpContextBase httpContext, string itemId);

        List<BasketItemVM> GetBasketItems(HttpContextBase httpContext);
        BasketSummaryVM GetBasketSummary(HttpContextBase httpContext);
    }
}