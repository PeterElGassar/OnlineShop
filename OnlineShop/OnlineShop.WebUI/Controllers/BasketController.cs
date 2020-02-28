using OnlineShop.Core.Contracts;
using OnlineShop.Core.ViewModels;
using OnlineShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        BasketServics _basketService;
        public BasketController(IBasketService basketService, BasketServics _basketService)
        {
            this.basketService = basketService;
            this._basketService = _basketService;
        }

        // GET: Basket/Index
        public ActionResult Index()
        {
            List<BasketItemVM> model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        // GET: Basket/AddToBasket
        public ActionResult AddToBasket(string productId)
        {
            basketService.AddToBasket(this.HttpContext, productId);

            return RedirectToAction("Index");
        }


        // GET: Basket/RemoveFromBasket
        public ActionResult RemoveFromBasket(string productId)
        {
            basketService.RemoveFromBasket(this.HttpContext, productId);

            return RedirectToAction("Index");
        }

        // GET: Basket/BasketSummary
        public PartialViewResult BasketSummary()
        {
            var basketSummary= basketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }
    }
}