using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.API.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class Repository<T>(DbContext context) : IRepository<T> where T : class
    {
        private readonly DbContext _context = context;

        public async Task<T> CreateAsync(T entity, CancellationToken ct = default)
        {
            await _context.Set<T>().AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Set<T>().ToListAsync(ct);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var existing = await _context.Set<T>().FindAsync(id, ct);
            return existing;
        }

        public async Task<T?> UpdateAsync(int id, T entity, CancellationToken ct = default)
        {
            var existing = await _context.Set<T>().FindAsync([id], ct);

            if (existing == null)
                return null;

            _context.Entry(existing).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync(ct);

            return existing;
        }
    }
}