using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineShop.Core.Contracts;
using OnlineShop.Core.Models;
using OnlineShop.WebUI.Tests.Mocks;
using OnlineShop.WebUI.Controllers;
using OnlineShop.Services;
using System.Linq;
using System.Web.Mvc;
using OnlineShop.Core.ViewModels;

namespace OnlineShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //setUp Test
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            var httpContext = new MockHttpContext();


            IBasketService basketService = new BasketServics(products, baskets);
            var controller = new BasketController(basketService);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            //basketService.AddToBasket(httpContext, "1");
            controller.AddToBasket("1");
            Basket basket = baskets.Collection().FirstOrDefault();


            //Assert
            //At Least Has One Basket
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count());
            Assert.AreEqual("1", basket.BasketItems.FirstOrDefault().ProductId);
        }


        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            //Sutup 
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Basket> Baskets = new MockContext<Basket>();

            //Add Some Product
            products.Insert(new Product() { Id = "1", Name = "Cell Phone", Price = 50.00m });
            products.Insert(new Product() { Id = "2", Name = "Tv", Price = 10.00m });
            products.Insert(new Product() { Id = "3", Name = "Jacket", Price = 10.00m });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "3", Quantity = 4 });
            //Add This Basket To Basket Collection
            Baskets.Insert(basket);

            IBasketService basketService = new BasketServics(products, Baskets);

            var controller = new BasketController(basketService);
            var httpContext = new MockHttpContext();

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });

            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Assert 
            var result = controller.BasketSummary() as PartialViewResult;
            //Check Model Type of view
            var basketSummary = (BasketSummaryVM)result.ViewData.Model;

            Assert.AreEqual(7,basketSummary.BasketCount);
            Assert.AreEqual(150.00m, basketSummary.BasketTotal);

        }
    }
}
