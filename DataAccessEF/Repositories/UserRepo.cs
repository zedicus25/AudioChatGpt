
using DataAccesEF;
using DataAccesEF.Data;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessEF.Repositories
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        public UserRepo(DbA966d8ChatgptContext dbContext) : base(dbContext)
        {
        }
    }
}
