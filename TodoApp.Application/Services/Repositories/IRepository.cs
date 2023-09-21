using System.Linq.Expressions;

namespace TodoApp.Application.Services.Repositories;

public interface IRepository<TEntity> where TEntity : class, new()
{
    Task<TEntity> GetAsync(long id, CancellationToken cancellation);

    Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation);
    
    Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellation);

    Task<bool> AddAsync(TEntity entity, CancellationToken cancellation);

    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellation);

    Task<bool> DeleteAsync(long id, CancellationToken cancellation);
}