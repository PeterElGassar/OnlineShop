using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.DataAccess.InMemory;
using OnlineShop.Core.Models;
using OnlineShop.Core;
using OnlineShop.Core.Contracts;

namespace OnlineShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        IRepository<ProductCategory> context;

        //Ctor Injected 
        public ProductCategoryManagerController(IRepository<ProductCategory> context)
        {
            this.context = context;
        }

        public ActionResult Index()
        {
            List<ProductCategory> ProductCategories = context.Collection().ToList(); ;
            return View(ProductCategories);
        }

        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();

            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {
                if (context.Collection().Where(c => c.Id != productCategory.Id).Any(c => c.Name == productCategory.Name))
                {
                    ModelState.AddModelError("NameNotUnique", "Category Name Is Exist Before");
                    return View(productCategory);
                }

                productCategory.Slug = productCategory.Name.ToLower().Replace(" ", "-").Replace(".", "-");

                context.Insert(productCategory);
                context.Commit();

                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(string slug)
        {
            ProductCategory productCategory = context.FindBySlug(slug);

            if (productCategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategory);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            //First Find Specific Item To Edit it
            ProductCategory CategoryToEdit = context.FindBySlug(productCategory.Slug);

            //Second Check it If Null Or Not
            if (CategoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCategory);
                }
                if (context.Collection().Where(c => c.Id != productCategory.Id).Any(c => c.Name == productCategory.Name))
                {
                    ModelState.AddModelError("NameNotUnique", "Category Name Is Exist Before");
                    return View(productCategory);
                }

                CategoryToEdit.Name = productCategory.Name;
                CategoryToEdit.Slug = productCategory.Name.ToLower().Replace(" ", "-").Replace(".", "-");

                context.Commit();
                return RedirectToAction("Edit", new { slug = CategoryToEdit.Slug });
            }
        }



        public ActionResult Delete(string slug)
        {
            ProductCategory CategoryToDelete = context.FindBySlug(slug);

            if (CategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(CategoryToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string slug)
        {
            ProductCategory CategoryToDelete = context.FindBySlug(slug);

            if (CategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.DeleteBySlug(slug);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}