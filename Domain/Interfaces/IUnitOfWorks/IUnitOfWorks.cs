namespace Domain.Interfaces.IUnitOfWorks
{
    public interface IUnitOfWorks : IDisposable
    {
        public IUserRepo UsersRepo { get; }

        int Commit();
    }
}
