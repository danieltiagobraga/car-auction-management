using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarAuctions.Data.Repositories.Base;

public abstract class Repository<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    private readonly DbSet<TEntity> _entities;

    protected Repository(DbContext context)
    {
        _context = context;
        _entities = _context.Set<TEntity>();
    }

    public TEntity? Get(string id)
        => _entities.Find(id);

    public IEnumerable<TEntity> GetAll()
        => _entities;

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        => _entities.Where(predicate);

    public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        => _entities.SingleOrDefault(predicate);

    public void Add(TEntity entity)
        => _entities.Add(entity);

    public void AddRange(IEnumerable<TEntity> entities)
        => _entities.AddRange(entities);

    public void Remove(TEntity entity)
        => _entities.Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities)
        => _entities.RemoveRange(entities);

    public void Update(TEntity entity)
        => _entities.Update(entity);
}
