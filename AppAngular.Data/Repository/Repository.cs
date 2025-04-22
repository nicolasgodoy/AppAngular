using AppAngular.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AppAngular.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity); // Agrega la entidad a la base de datos
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
        }

        public Task DeleteAsync(T entityDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entityDto)
        {
            throw new NotImplementedException();
        }
    }
}
