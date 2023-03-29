using DataAccesEF.Data;
using DataAccessEF.Repositories;
using Domain.Interfaces;
using Domain.Interfaces.IUnitOfWorks;


namespace DataAccessEF.UnitOfWorks
{

    public class UnitOfWorks : IUnitOfWorks
    {
        public IUserRepo UsersRepo { get; }

        public ISubscriptionRepo SubscriptionRepo { get; }

        public IHistoryRepo HistoryRepo { get; }

        public IRequestRepo RequestRepo { get; }

        public IResponceRepo ResponceRepo { get; }

        private readonly DbA966d8ChatgptContext _dbContext;

        public UnitOfWorks(DbA966d8ChatgptContext context)
        {
            _dbContext = context;
            UsersRepo = new UserRepo(context);
            SubscriptionRepo = new SubsriptionRepo(context);
            HistoryRepo = new HistoryRepo(context); 
            RequestRepo = new RequestRepo(context);
            ResponceRepo = new ResponceRepo(context);
        }
        public int Commit() => _dbContext.SaveChanges();

        public void Dispose() => _dbContext.Dispose();
    }
}
