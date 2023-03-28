using DataAccesEF.Data;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessEF.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        protected readonly DbA966d8ChatgptContext _dbContext;

        public GenericRepo(DbA966d8ChatgptContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public virtual void Add(T item) => _dbContext.Set<T>().Add(item);


        public virtual void Delete(int id)
        {
            var itemForDelete = _dbContext.Set<T>().Find(id);
            _dbContext.Set<T>().Remove(itemForDelete);
        }

        public virtual async Task<IEnumerable<T>> GetAll() => await _dbContext.Set<T>().ToListAsync();

        public virtual void Update(T item) => _dbContext.Set<T>().Update(item);

    }
}
