using OnlineShop.Core.Contracts;
using OnlineShop.Core.Models;
using OnlineShop.Core.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineShop.Services
{
    public class BasketServies : IBasketServies
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketServies(IRepository<Product> productContext, IRepository<Basket> basketContext)
        {
            this.productContext = productContext;
            this.basketContext = basketContext;
        }

        //Privet fuction to check If Cookies Exist Or Not 
        private Basket GetBasket(HttpContextBase httpContext, bool createIfNll)
        {
            //Get All Cookies Request for current http request
            //Then And get Return Cookie with Specified Name 
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            //New Basket Object 
            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;
                //If basketId Not Null
                if (!string.IsNullOrEmpty(basketId))
                {
                    //get Specified Basket With This BasketId
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    //In This Case Create New Basket Because it's Not Exist yet
                    if (createIfNll)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }

            }
            else
            {
                //In This Case Create New Basket Because it's Not Exist yet
                if (createIfNll)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            //first Create New Basket And Add it To BD
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            //Last Thing Add This Cooke To Reponse
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            //خد ال الحالي http request
            Basket basket = GetBasket(httpContext, true);
            BasketItem singleItem = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (singleItem == null)
            {
                //In This Case This Product Not Exist In Basket 
                singleItem = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                //Add This New Item To BasketItems Of This Current Basket
                basket.BasketItems.Add(singleItem);
            }
            else
            {
                // if This Item Already Exist Just Increment
                singleItem.Quantity++;
            }
            //Save Changes
            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);

            BasketItem singleItem = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);
            if (singleItem != null)
            {
                basket.BasketItems.Remove(singleItem);
                basketContext.Commit();
            }

        }

        public List<BasketItemVM> GetBasketItems(HttpContextBase httpContext)
        {
            //Get Basket
            //Here IAm Path False Because I don't need Create New Basket 
            var basket = GetBasket(httpContext, false);

            if (basket != null)
            {
                //Query Throughtout basket to get specified infromation
                var results = (
                             from b in basket.BasketItems

                             join p in productContext.Collection()
                             on b.ProductId equals p.Id
                             select new BasketItemVM()
                             {
                                 BasketItemId = b.Id,
                                 Quentity = b.Quantity,
                                 //Product Part
                                 ProductName = p.Name,
                                 Price = p.Price,
                                 Image = p.Image
                             }).ToList();

                return results;
            }
            else
            {
                return new List<BasketItemVM>();
            }
        }


        public BasketSummaryVM GetBasketSummary(HttpContextBase httpContext)
        {
            //Get Basket
            //Here IAm Path False Because I don't need Create New Basket 
            var basket = GetBasket(httpContext, false);
            var model = new BasketSummaryVM(0, 0);
            List<BasketItemVM> getBasketItems = GetBasketItems(httpContext);

            if (basket != null)
            {
                if (getBasketItems != null)
                {
                    int? count = 0;
                    decimal? total = 0;
                    foreach (var item in getBasketItems)
                    {
                        count = +item.Quentity;
                        total = +item.Quentity * item.Price;

                    }
                    model.BasketCount = count ?? 0;
                    model.BasketTotal = total ?? decimal.Zero;
                }
               

                //int? basketCount = (from item in basket.BasketItems
                //                    select item.Quantity).Sum();



                //decimal? basketTotal = (from item in basket.BasketItems
                //                        join p in productContext.Collection()
                //                        on item.ProductId equals p.Id
                //                        select item.Quantity * p.Price).Sum();

                //model.BasketCount = basketCount ?? 0;
                //model.BasketTotal = basketTotal ?? decimal.Zero;

                return model;
            }
            else
            {
                return model;
            }
        }
    }
}
