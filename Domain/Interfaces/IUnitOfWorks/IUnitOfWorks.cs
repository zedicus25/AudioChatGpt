namespace Domain.Interfaces.IUnitOfWorks
{
    public interface IUnitOfWorks : IDisposable
    {
        public IUserRepo UsersRepo { get; }
        public ISubscriptionRepo SubscriptionRepo { get; }

        int Commit();
    }
}
