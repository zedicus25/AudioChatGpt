using DataAccesEF.Data;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccessEF.Repositories
{
    public class SubsriptionRepo : GenericRepo<Subscription>, ISubscriptionRepo
    {
        public SubsriptionRepo(DbA966d8ChatgptContext dbContext) : base(dbContext)
        {
        }

        public Subscription GetSubscriptionById(int id) =>
            _dbContext.Subscriptions.FirstOrDefault(x => x.Id == id);

    }
}
