using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Travellour.Core.Entities.Base;
using Travellour.Core.Interfaces;
using Travellour.Data.DAL;

namespace Travellour.Data.Implementations;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, new()
{
    private readonly AppDbContext? _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, params string[] includes)
    {
        var query = GetQuery(includes);

#pragma warning disable CS8603 // Possible null reference return.
        return predicate is null
            ? await query.FirstOrDefaultAsync()
            : await query.Where(predicate).FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async Task<List<TEntity>> GetAllAsync<TOrderBy>(Expression<Func<TEntity, TOrderBy>> orderBy, Expression<Func<TEntity, bool>>? predicate = null, params string[] includes)
    {
        var query = GetQuery(includes);

        return predicate is null
            ? await query.OrderByDescending(orderBy).ToListAsync()
            : await query.OrderByDescending(orderBy).Where(predicate).ToListAsync();
    }

    public async Task CreateAsync(TEntity entity)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var entry = _context.Entry(entity);
        entry.State = EntityState.Added;
        await _context.SaveChangesAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public async Task UpdateAsync(TEntity entity)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var entry = _context.Entry(entity);
        entry.State = EntityState.Modified;
        await _context.SaveChangesAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public async Task DeleteAsync(TEntity entity)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var entry = _context.Entry(entity);
        entry.State = EntityState.Deleted;
        await _context.SaveChangesAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    private IQueryable<TEntity> GetQuery(params string[] includes)
    {
        var query = _context?.Set<TEntity>().AsQueryable();
        if (includes is not null)
        {
            foreach (var item in includes)
            {
                query = query?.Include(item);
            }
        }
#pragma warning disable CS8603 // Possible null reference return.
        return query;
#pragma warning restore CS8603 // Possible null reference return.
    }
}
