﻿using System;
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
                context.Insert(productCategory);
                context.Commit();

                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(string id)
        {
            ProductCategory productCategory = context.Find(id);

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
        public ActionResult Edit(ProductCategory productCategory, string id)
        {
            //First Find Specific Item To Edit id
            ProductCategory CategoryToEdit = context.Find(id);

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
                CategoryToEdit.Name = productCategory.Name;
              
                context.Commit();
                return RedirectToAction("Index");
            }
        }



        public ActionResult Delete(string id)
        {
            ProductCategory CategoryToDelete = context.Find(id);

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
        public ActionResult ConfirmDelete(string id)
        {
            ProductCategory CategoryToDelete = context.Find(id);

            if (CategoryToDelete == null)
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