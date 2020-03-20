using System.Linq;

namespace Base.Services.Abstract
{
    public interface IBaseObjectService<T> where T : BaseObject
    {
        T Get(int id);
        T Create(T obj);
        T Update(T obj);
        void Delete(T obj);
        void Delete(int id);

        IQueryable<T> All();
        //TODO Реализовать такие же методы под коллекции
    }
}
