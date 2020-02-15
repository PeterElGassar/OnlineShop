using OnlineShop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;

        List<ProductCategory> productCategories;

        //Ctor
        public ProductCategoryRepository()
        {
            //first cast cache Object If can convert it to type  List<Product>
            //if not products is null 
            productCategories = cache["products"] as List<ProductCategory>;


            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }


        //Insert New Category
        public void Insert(ProductCategory category)
        {
            productCategories.Add(category);
        }

        //Update Category
        public void Update(ProductCategory category)
        {
            ProductCategory getCategoryToUpdate = productCategories.Find(p => p.Id == category.Id);

            if (getCategoryToUpdate != null)
            {
                getCategoryToUpdate = category;
            }
            else
            {
                throw new Exception("Product Category Not Found");
            }

        }

        //Find Single Category
        public ProductCategory find(string id)
        {
            var getCategory = productCategories.Find(p => p.Id == id);

            if (getCategory != null)
            {
                return getCategory;
            }
            else
            {
                throw new Exception("Product Category Not Found");
            }

        }

        //Find Collection Of Categories
        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        //  مش محتاجة شرح ياعني بتعمل ايه (:
        public void Delete(string id)
        {
            ProductCategory getCategoryToDel = productCategories.Find(p => p.Id == id);

            if (getCategoryToDel != null)
            {
                productCategories.Remove(getCategoryToDel);
            }
            else
            {
                throw new Exception("Product Category Not Found");
            }
        }

        //Save Changes In Cache Object
        public void Commit()
        {
            cache["products"] = productCategories;
        }


    }
}
