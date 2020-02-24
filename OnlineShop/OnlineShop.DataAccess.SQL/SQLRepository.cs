using OnlineShop.Core.Contracts;
using OnlineShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {

        internal DataContext _context;
        internal DbSet<T> _dbSet;
        public SQLRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }




        public IQueryable<T> Collection()
        {
            return _dbSet;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var item = Find(id);
            if (_context.Entry(item).State == EntityState.Deleted)
            {
                _dbSet.Attach(item);
            }
            _dbSet.Remove(item);
        }

        public T Find(string id)
        {
            return _dbSet.Find(id);
        }

        public T FindBySlug(string slug)
        {
            return _dbSet.Find(slug);
        }


        public void Insert(T item)
        {
            _dbSet.Add(item);
        }

        public void Update(T item)
        {
            _dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
