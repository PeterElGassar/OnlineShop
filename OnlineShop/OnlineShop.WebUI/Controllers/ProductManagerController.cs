using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Core.Models;
using OnlineShop.Core.ViewModels;
using OnlineShop.DataAccess.InMemory;
using OnlineShop.Core.Contracts;
using System.IO;
using System.Web.Helpers;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OnlineShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategoris;

        //Ctor Inject Tow Interfaces To Every Instance
        public ProductManagerController(IRepository<Product> productContext,
            IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategoris = productCategoryContext;
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
            viewModel.Categories = productCategoris.Collection().ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(ProductManagerVM viewModel, HttpPostedFileBase file)
        {
            viewModel.Categories = productCategoris.Collection().ToList();

            //First Check Model State 
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string id = viewModel.Product.Id;
            //Second Make sure the name does not repeat
            if (context.Collection().Where(p => p.Id != id).Any(p => p.Name == viewModel.Product.Name))
            {
                //product.cate
                ModelState.AddModelError("NameNotUnique", "Product Name Is Exist Before");
                return View(viewModel);
            }
            #region Insert Image Of Product


            if (file != null && file.ContentLength > 0)
            {
                string imageExtention = Path.GetExtension(file.FileName).ToLower();

                string[] vaildTypes = { ".jpg", ".jpeg", ".png", ".gif" };

                if (vaildTypes.Any(item => item == imageExtention))
                {

                    try
                    {
                        //Main Directory For All Products
                        //Main Directory Must Exists from before
                        var origenalDirectory = new DirectoryInfo(Server.MapPath(@"\") + @"Images\Uploads");

                        //Create Some Folders To Every Single Product
                        string productsPath = Path.Combine(origenalDirectory.ToString(), "Products");

                        string singleProductPath = Path.Combine(origenalDirectory.ToString(), @"Products\" + viewModel.Product.Id);

                        string productThumbPath = Path.Combine(origenalDirectory.ToString(), @"Products\" + viewModel.Product.Id + @"\Thumbs");

                        string productGalleryPath = Path.Combine(origenalDirectory.ToString(), @"Products\" + viewModel.Product.Id + @"\Gallery");

                        string productGalleryThumbsPath = Path.Combine(origenalDirectory.ToString(), @"Products\" + viewModel.Product.Id + @"\Gallery\Thumbs");
                        //Exist All Path IF Not Exists yet
                        if (!Directory.Exists(productsPath))
                            Directory.CreateDirectory(productsPath);

                        if (!Directory.Exists(singleProductPath))
                            Directory.CreateDirectory(singleProductPath);

                        if (!Directory.Exists(productThumbPath))
                            Directory.CreateDirectory(productThumbPath);

                        if (!Directory.Exists(productGalleryPath))
                            Directory.CreateDirectory(productGalleryPath);

                        if (!Directory.Exists(productGalleryThumbsPath))
                            Directory.CreateDirectory(productGalleryThumbsPath);

                        //======================================================//
                        string imageName = file.FileName;
                        viewModel.Product.Image = imageName;
                        //======================Set Original Images ================================//
                        string orignalImage = string.Format(@"{0}\{1}", singleProductPath, imageName);
                        file.SaveAs(orignalImage);
                        //======================Set Thumb Images ================================//
                        string thumbIamge = string.Format(@"{0}\{1}", productThumbPath, imageName);
                        WebImage img = new WebImage(file.InputStream);
                        img.Resize(480, 640, true, true).Crop(1, 1, 1, 1).Write();
                        img.Save(thumbIamge);
                    }
                    catch (Exception ex)
                    {

                        TempData["Error"] = ex.Message;
                        return View(viewModel);
                    }
                }
            }
            #endregion

            //viewModel.Product.Slug = viewModel.Product.Name.ToLower().Replace(" ", "-").Replace(".", "-");
            Product product = new Product()
            {
                Id = viewModel.Product.Id,
                Name = viewModel.Product.Name,
                Description = viewModel.Product.Description,
                Slug = viewModel.Product.Name.ToLower().Replace(" ", "-").Replace(".", "-"),
                Price = viewModel.Product.Price,
                Image = viewModel.Product.Image,
                ProductCategoryId = viewModel.Product.ProductCategoryId,
            };
            context.Insert(product);
            context.Commit();

            TempData["Message"] = "Product Add Successfully";
            return RedirectToAction("Create");

        }

        public ActionResult Edit(string slug)
        {
            Product product = context.FindBySlug(slug);

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerVM viewModel = new ProductManagerVM();
                viewModel.Product = product;
                viewModel.Categories = productCategoris.Collection();

                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductManagerVM viewModel, HttpPostedFileBase file)
        {
            //First Thing Should Do Fill DropDown 
            string id = viewModel.Product.Id;
            string slug = viewModel.Product.Slug;
            viewModel.Categories = productCategoris.Collection().ToList();
            // Find Specific Item To Edit id
            Product productToEdit = context.FindBySlug(slug);

            //Second Check it If Null Or Not
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                //First Check Model State 
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }
                //Second Make sure the name does not repeat
                var proList = context.Collection().Where(p => p.Id != id);
                if (proList.Any(p => p.Name == viewModel.Product.Name))
                {
                    //product.cate
                    ModelState.AddModelError("NameNotUnique", "Product Name Is Exist Before");
                    return View(viewModel);
                }
                productToEdit.Name = viewModel.Product.Name;
                productToEdit.Category = viewModel.Product.Category;
                productToEdit.Description = viewModel.Product.Description;
                productToEdit.Price = viewModel.Product.Price;
                productToEdit.Image = viewModel.Product.Image;
                productToEdit.ProductCategoryId = viewModel.Product.ProductCategoryId;
                productToEdit.Slug = viewModel.Product.Name.ToLower().Replace(" ", "-").Replace(".", "-");

                #region Update Image
                if (file != null && file.ContentLength > 0)
                {
                    var orignalPath = new DirectoryInfo(Server.MapPath(@"\Images\Uploads"));

                    string stringPath1 = Path.Combine(orignalPath.ToString() + "\\Products\\" + id.ToString());
                    string StringPath2 = Path.Combine(orignalPath.ToString() + "\\Products\\" + id.ToString() + "\\Thumbs");

                    DirectoryInfo direct1 = new DirectoryInfo(stringPath1);
                    DirectoryInfo direct2 = new DirectoryInfo(StringPath2);

                    try
                    {
                        //Get the content of the files and delete them all
                        foreach (var file1 in direct1.GetFiles())
                        {
                            file1.Delete();
                        }
                        foreach (var file2 in direct2.GetFiles())
                        {
                            file2.Delete();
                        }


                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = ex.Message;
                        return View(viewModel);
                    }
                    finally
                    {
                        //New Image Data
                        string imageName = file.FileName;
                        productToEdit.Image = imageName;

                        string path1 = string.Format(@"{0}\\{1}", stringPath1, imageName);
                        file.SaveAs(path1);

                        string path2 = string.Format(@"{0}\\{1}", StringPath2, imageName);
                        WebImage img = new WebImage(file.InputStream);
                        img.Resize(480, 640, true, true).Crop(1, 1, 1, 1).Write();
                        img.Save(path2);
                    }
                }

                #endregion
                context.Commit();
                TempData["Message"] = "Product Updated Successfully";
                return RedirectToAction("Edit", new { slug = productToEdit.Slug });

            }




        }



        public ActionResult Delete(string slug)
        {
            Product productToDelete = context.FindBySlug(slug);

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
        public ActionResult ConfirmDelete(string slug)
        {
            Product productToDelete = context.FindBySlug(slug);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.DeleteBySlug(slug);

                #region Delete Image
                var orignalDirectory = new DirectoryInfo(Server.MapPath(@"\Images\Uploads\Products"));

                string path1 = Path.Combine(orignalDirectory.ToString() + "\\" + productToDelete.Id);


                if (Directory.Exists(path1))
                {
                    Directory.Delete(path1, true);
                }
                #endregion

                //====================saveChanges==============
                context.Commit();
                return RedirectToAction("Index");
            }

        }


    }
}










//thumbNail.Save(thumbIamge);
//using (Image img = Image.FromFile(orgFile))
//{
//    Image thumbNail = new Bitmap(width, height, img.PixelFormat);
//    Graphics g = Graphics.FromImage(thumbNail);
//    g.CompositingQuality = CompositingQuality.HighQuality;
//    g.SmoothingMode = SmoothingMode.HighQuality;
//    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
//    Rectangle rect = new Rectangle(0, 0, width, height);
//    g.DrawImage(img, rect);
//    thumbNail.Save(resizedFile, format);
//}