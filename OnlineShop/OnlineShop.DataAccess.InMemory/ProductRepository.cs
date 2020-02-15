using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using OnlineShop.Core;
using OnlineShop.Core.Models;

namespace OnlineShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            //first cast cache Object If can convert it to type  List<Product>
            //if not products is null 
            products = cache["products"] as List<Product>;


            if (products == null)
            {
                products = new List<Product>();
            }
        }

        public void Commit()
        {
            cache["products"] = products;
        }

        public void Insert(Product product)
        {
            products.Add(product);
        }

        public void Update(Product product)
        {
            Product getProductToUpdate = products.Find(p => p.Id == product.Id);

            if (getProductToUpdate != null)
            {
                getProductToUpdate = product;
            }
            else
            {
                throw new Exception("Product Not Found");
            }

        }

        public Product find(string id)
        {
            var getProduct = products.Find(p => p.Id == id);
            if (getProduct != null)
            {
                return getProduct;
            }
            else
            {
                throw new Exception("Product Not Found");
            }

        }

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }


        public void Delete(string id)
        {
            Product getProductToDel = products.Find(p => p.Id == id);

            if (getProductToDel != null)
            {
                products.Remove(getProductToDel);
            }
            else
            {
                throw new Exception("Product Not Found");
            }
        }


    }
}
