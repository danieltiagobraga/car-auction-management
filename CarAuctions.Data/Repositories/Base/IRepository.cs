﻿using System.Linq.Expressions;

namespace CarAuctions.Data.Repositories.Base;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity? Get(string id);
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

    TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
}
