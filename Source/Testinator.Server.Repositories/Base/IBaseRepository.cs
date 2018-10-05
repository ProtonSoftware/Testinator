using System.Linq;
using Testinator.Server.DataAccess.Entities.Base;

namespace Testinator.Server.Repositories.Base
{
    public interface IRepository<T, K> where T : IBaseObject<K>, new()
    {
        void Add(T entity);

        void Delete(T entity);

        void Delete(K id);

        T GetById(K id);

        IQueryable<T> GetAll();

        bool SaveChanges();
    }
}
