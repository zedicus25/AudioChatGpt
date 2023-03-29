using Domain.Models;

namespace Domain.Interfaces
{
    public interface ISubscriptionRepo : IGenericRepo<Subscription>
    {
        Subscription GetSubscriptionById(int id);
    }
}
