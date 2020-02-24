using OnlineShop.Core.Contracts;
using OnlineShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {

        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;
        //Ctor
        public InMemoryRepository()
        {
            className = typeof(T).Name;

            items = cache[className] as List<T>;

            if (items == null)
            {
                items = new List<T>();

            }
        }
        //  Commit Method Used Instead Of Save In DataBase
        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T item)
        {
            items.Add(item);
        }

        public void Update(T item)
        {
            T getItemToUpdate = items.Find(i => i.Id == item.Id);

            if (getItemToUpdate != null)
            {
                getItemToUpdate = item;
            }
            else
            {
                throw new Exception(className + "Not Found");
            }
        }

        public T Find(string id)
        {
            T getItem = items.Find(i => i.Id == id);

            if (getItem != null)
            {
                return getItem;
            }
            else
            {
                throw new Exception(className + "Not Found");
            }
        }

        public T FindBySlug(string slug)
        {
            T getItem = items.Find(i => i.Slug == slug);

            if (getItem != null)
            {
                return getItem;
            }
            else
            {
                throw new Exception(className + "Not Found");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable<T>();
        }

        public void Delete(string id)
        {
            T getItemToDel = items.Find(i => i.Id == id);

            if (getItemToDel != null)
            {
                items.Remove(getItemToDel);
            }
            else
            {
                throw new Exception(className + "Not Found");
            }
        }
        public void DeleteBySlug(string slug)
        {
            T getItemToDel = items.Find(i => i.Id == slug);

            if (getItemToDel != null)
            {
                items.Remove(getItemToDel);
            }
            else
            {
                throw new Exception(className + "Not Found");
            }
        }
        public void test() { }
    }
}
