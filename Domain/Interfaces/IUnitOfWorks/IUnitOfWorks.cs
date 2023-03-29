namespace Domain.Interfaces.IUnitOfWorks
{
    public interface IUnitOfWorks : IDisposable
    {
        public IUserRepo UsersRepo { get; }
        public ISubscriptionRepo SubscriptionRepo { get; }
        public IHistoryRepo HistoryRepo { get; }
        public IRequestRepo RequestRepo { get; }
        public IResponceRepo ResponceRepo { get; }

        int Commit();
    }
}
