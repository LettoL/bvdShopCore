using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Base
{
    public class EfRepository<T> where T : BaseObject
    {
        private readonly DbContext _context;

        public EfRepository(DbContext context)
        {
            _context = context;
        }

        public virtual IQueryable<T> GetAll()
        {
            var a = _context.Set<T>().ToList();

            return _context.Set<T>();
        }
    }
}
