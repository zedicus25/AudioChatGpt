namespace Domain.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        void Add(T item);
        void Delete(int id);
        void Update(T item);
    }
}
