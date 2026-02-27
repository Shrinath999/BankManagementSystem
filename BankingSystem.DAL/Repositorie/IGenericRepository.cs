namespace BankingSystem.DAL.Repositorie
{
    public interface IGenericRepository
    {

        public interface IGenericRepository<T> where T : class
        {
            Task<IEnumerable<T>> GetAllAsync();
            Task<T> GetByIdAsync(int id);
            Task InsertAsync(T entity);
            void Update(T entity);
            void Delete(T entity);
            Task SaveAsync();
        }
    }
}
