using Domain.Models;


namespace Domain.Interfaces
{
    public interface IUserRepo : IGenericRepo<User>
    {
        Task<User> FindByLoginAsync(string login);

        bool CheckPassword(User user, string password);
    }
}
