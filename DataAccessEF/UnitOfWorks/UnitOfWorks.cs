using DataAccesEF.Data;
using DataAccessEF.Repositories;
using Domain.Interfaces;
using Domain.Interfaces.IUnitOfWorks;


namespace DataAccessEF.UnitOfWorks
{
    public class UnitOfWorks : IUnitOfWorks
    {
        public IUserRepo UsersRepo { get; }

        private readonly DbA966d8ChatgptContext _dbContext;

        public UnitOfWorks(DbA966d8ChatgptContext context)
        {
            _dbContext = context;
            UsersRepo = new UserRepo(context);
        }
        public int Commit() => _dbContext.SaveChanges();

        public void Dispose() => _dbContext.Dispose();
    }
}
