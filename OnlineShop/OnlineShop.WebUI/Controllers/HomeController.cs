using OnlineShop.Core.Contracts;
using OnlineShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        public HomeController(IRepository<Product> context, IRepository<ProductCategory> productCategories)
        {
            this.context = context;
            this.productCategories = productCategories;
        }

        public ActionResult Index()
        {
            List<Product> productList = context.Collection().ToList();
            return View(productList);
        }

        public ActionResult Details(string id)
        {
            var product = context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }


            return View(product);
        }

    }
}