using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineShop.Core.Contracts;
using OnlineShop.Core.Models;
using OnlineShop.WebUI.Controllers;
using OnlineShopTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OnlineShopTest.Controllers
{
    [TestClass]
   
    class Home
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            IRepository<Product> productContext = new MockContext<Product>();
            IRepository<ProductCategory> CategoryContext = new MockContext<ProductCategory>();

            HomeController controller = new HomeController(productContext , CategoryContext);

            var result = controller.Index() as ViewResult;
        }
    }
}
