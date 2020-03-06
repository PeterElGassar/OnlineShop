using OnlineShop.Core.Contracts;
using OnlineShop.Core.Models;
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
        IOrderService orderService;
        public BasketController(IBasketService basketService, IOrderService orderService)
        {
            this.basketService = basketService;
            this.orderService = orderService;
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
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);

            return PartialView(basketSummary);
        }

        public ActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(Order order)
        {
            //Get  All Items In Current Basket From Basket Sevice Layer
            var basketItem = basketService.GetBasketItems(this.HttpContext);
            order.OrderState = "Order created";
            //process payment
            order.OrderState = "Payment proccessed";

            orderService.CreateOrder(order, basketItem);
            //Finaly clear basket
            basketService.ClearBasketItems(this.HttpContext);

            return RedirectToAction("ThankYou", new { orderId = order.Id });
        }


        public ActionResult ThankYou(string orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}