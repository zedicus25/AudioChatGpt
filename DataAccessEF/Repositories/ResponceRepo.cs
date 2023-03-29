using DataAccesEF.Data;
using Domain.Interfaces;
using Domain.Models;


namespace DataAccessEF.Repositories
{
    public class ResponceRepo : GenericRepo<Responce>, IResponceRepo
    {
        public ResponceRepo(DbA966d8ChatgptContext dbContext) : base(dbContext)
        {
        }
    }
}
