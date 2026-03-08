using System.Linq.Expressions;
using BankingSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
using static BankingSystem.DAL.Repositorie.IGenericRepository;

namespace BankingSystem.DAL.Repositorie
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BankingDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(BankingDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }
    }
}
