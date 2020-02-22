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
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }


            #region Insert Image Of Product
            string imageExtention = Path.GetExtension(file.FileName).ToLower();

            string[] vaildTypes = { ".jpg", ".jpeg", ".png", ".gif" };

            if (file != null && file.ContentLength > 0)
            {
                if (vaildTypes.Any(item => item == imageExtention))
                {


                    //Main Directory For All Products
                    //Main Directory Must Exists
                    var origenalDirectory = new DirectoryInfo(Server.MapPath(@"\") + @"Images\Uploads");

                    //Create Some Folders To Every Single Product
                    string productsPath = Path.Combine(origenalDirectory.ToString(), "Products");

                    string singleProductPath = Path.Combine(origenalDirectory.ToString(), @"Products\" + product.Id);

                    string productThumbPath = Path.Combine(origenalDirectory.ToString(), @"Products\" + product.Id + @"\Thumbs");

                    string productGalleryPath = Path.Combine(origenalDirectory.ToString(), @"Products\" + product.Id + @"\Gallery");

                    string productGalleryThumbsPath = Path.Combine(origenalDirectory.ToString(), @"Products\" + product.Id + @"\Gallery\Thumbs");
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
                    product.Image = imageName;
                    //======================Set Original Images ================================//
                    string orignalImage = string.Format(@"{0}\{1}", singleProductPath, imageName);
                    file.SaveAs(orignalImage);
                    //======================Set Thumb Images ================================//
                    string thumbIamge = string.Format(@"{0}\{1}", productThumbPath, imageName);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(480, 640, true, true).Crop(1, 1, 1, 1).Write();
                    img.Save(thumbIamge);

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

            }

            #endregion

            context.Insert(product);
            context.Commit();

            TempData["Message"] = "Product Add Successfully";
            return RedirectToAction("Index");

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
                viewModel.Categories = productCategoris.Collection();

                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductManagerVM viewModel, string id, HttpPostedFileBase file)
        {
            //First Thing Should Do Fill DropDown 
            viewModel.Categories = productCategoris.Collection().ToList();
            // Find Specific Item To Edit id
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
                    return View(viewModel);
                }

                productToEdit.Name = viewModel.Product.Name;
                productToEdit.Category = viewModel.Product.Category;
                productToEdit.Description = viewModel.Product.Description;
                productToEdit.Price = viewModel.Product.Price;
                productToEdit.Image = viewModel.Product.Image;

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
                    catch (Exception ex)
                    {
                        TempData["Error"] = ex.Message;

                        return View(viewModel);
                    }
                }

                #endregion
                context.Commit();
                TempData["Message"] = "Product Updated Successfully";
                return RedirectToAction("Edit");

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