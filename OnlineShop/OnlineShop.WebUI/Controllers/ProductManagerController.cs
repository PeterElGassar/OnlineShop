using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Core.Models;
using OnlineShop.Core.ViewModels;
using OnlineShop.DataAccess.InMemory;

namespace OnlineShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        InMemoryRepository<Product> context;
        InMemoryRepository<ProductCategory> ProductCategoris;

        public ProductManagerController()
        {
            context = new InMemoryRepository<Product>();
            ProductCategoris = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> Products = context.Collection().ToList();
            return View(Products);
        }

        public ActionResult Create()
        {
            ProductManagerVM viewModel = new ProductManagerVM();

            viewModel.Product = new Product();
            viewModel.Categories = ProductCategoris.Collection().ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(string id)
        {
            Product product = context.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerVM viewModel = new ProductManagerVM();
                viewModel.Product = product;
                viewModel.Categories = ProductCategoris.Collection();

                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string id)
        {
            //First Find Specific Item To Edit id
            Product productToEdit = context.Find(id);

            //Second Check it If Null Or Not
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                productToEdit.Name = product.Name;
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Price = product.Price;
                productToEdit.Image = product.Image;

                context.Commit();
                return RedirectToAction("Index");
            }
        }



        public ActionResult Delete(string id)
        {
            Product productToDelete = context.Find(id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            Product productToDelete = context.Find(id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }


    }
}