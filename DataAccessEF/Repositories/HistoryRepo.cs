using DataAccesEF.Data;
using Domain.Interfaces;
using Domain.Models;


namespace DataAccessEF.Repositories
{
    public class HistoryRepo : GenericRepo<History>, IHistoryRepo
    {
        public HistoryRepo(DbA966d8ChatgptContext dbContext) : base(dbContext)
        {
        }
    }
}
