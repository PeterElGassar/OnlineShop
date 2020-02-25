using OnlineShop.Core.Contracts;
using OnlineShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Core.ViewModels;
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

        public ActionResult Index(string category = null)
        {
            List<Product> productList;
            List<ProductCategory> categoris = productCategories.Collection().ToList();
            if (category == null)
            {
                productList = context.Collection().ToList();
            }
            else
            {
                productList = context.Collection().Where(p => p.ProductCategory.Slug == category).ToList();
            }

            var model = new ProductListVM()
            {
                Products = productList,
                Categories = categoris
            };
            return View(model);
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