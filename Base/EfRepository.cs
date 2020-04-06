using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Base
{
    public class EfRepository<T> where T : BaseObject
    {
        public virtual IQueryable<T> GetAll(DbContext _context)
        {
            var a = _context.Set<T>().ToList();

            return _context.Set<T>();
        }
    }
}
