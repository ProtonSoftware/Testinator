using Microsoft.EntityFrameworkCore;
using System.Linq;
using Testinator.Server.DataAccess;
using Testinator.Server.DataAccess.Entities.Base;

namespace Testinator.Server.Repositories.Base
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseRepository<T, K> : IRepository<T, K> where T : class, IBaseObject<K>, new()
    {
        protected TestinatorAppDataContext Db { get; set; }

        protected abstract DbSet<T> DbSet { get; }

        public BaseRepository(TestinatorAppDataContext db)
        {
            Db = db;
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Delete(K id)
        {
            var entity = new T()
            {
                Id = id,
            };
            Db.Entry(entity).State = EntityState.Deleted;
        }

        public T GetById(K id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public bool SaveChanges()
        {
            try
            {
                var changes = Db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // TODO: Foregin key error handling if we even care
                /*if (ex.ForeginKeyViolation())
                    return new OperationResult("Foregin key violation!");
                */
                throw ex;
            }

            return true;
        }
    }
}
