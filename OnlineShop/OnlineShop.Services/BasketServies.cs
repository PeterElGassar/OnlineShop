using OnlineShop.Core.Contracts;
using OnlineShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineShop.Services
{
    public class BasketServies
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
    }
}
