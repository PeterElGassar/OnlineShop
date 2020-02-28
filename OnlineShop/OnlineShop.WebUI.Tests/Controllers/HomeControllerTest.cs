using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineShop.WebUI;
using OnlineShop.WebUI.Controllers;
using OnlineShop.WebUI.Tests.Mocks;
using OnlineShop.Core.Models;
using OnlineShop.Core.Contracts;

namespace OnlineShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            IRepository<Product> productContext = new MockContext<Product>();
            IRepository<ProductCategory> CategoryContext = new MockContext<ProductCategory>();
            // Arrange
            HomeController controller = new HomeController(productContext, CategoryContext);

            //var result = controller.Index() as ViewResult;

        }


    }
}
