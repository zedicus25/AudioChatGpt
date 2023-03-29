using DataAccesEF.Data;
using Domain.Interfaces;
using Domain.Models;


namespace DataAccessEF.Repositories
{
    public class RequestRepo : GenericRepo<Request>, IRequestRepo
    {
        public RequestRepo(DbA966d8ChatgptContext dbContext) : base(dbContext)
        {
        }
    }
}
