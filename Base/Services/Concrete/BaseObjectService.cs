using System;
using System.Linq;
using Base.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Base.Services.Concrete
{
    public class BaseObjectService<T> : IBaseObjectService<T> where T : BaseObject
    {
        private readonly DbContext _context;

        public BaseObjectService(DbContext context)
        {
            _context = context;
        }

        private DbSet<T> DbSet => _context.Set<T>();

        public virtual T Get(int id)
        {
            T obj = DbSet.Find(id);
            if(obj == null)
                throw new Exception(typeof(T) + " is null");

            return obj;
        }

        public virtual T Create(T obj)
        {
            var o = DbSet.Add(obj).Entity;
            _context.SaveChanges();
            return o;
        }

        public virtual T Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();
            return Get(obj.Id);
        }

        public virtual void Delete(T obj)
        {
            DbSet.Remove(obj);
            _context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var obj = Get(id);
            Delete(obj);
        }

        public virtual IQueryable<T> All()
        {
            return DbSet;
        }
    }
}
