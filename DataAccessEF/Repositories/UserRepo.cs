
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

        public override void Add(User item)
        {
            string hashedPassword = PasswordHasher.HashPassword(item.Password);
            item.Password = hashedPassword;

             _dbContext.Users.Add(item);
        }
        public bool CheckPassword(User user, string password)
        {
           return PasswordHasher.VerifyHashedPassword(user.Password, password);
        }

        public async Task<User> FindByLoginAsync(string login) => 
             await _dbContext.Users.FirstOrDefaultAsync(x => x.Login.Equals(login));

    }
}
