using System;
using System.Threading.Tasks;

namespace Orders.Core.Shared;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    Task<TEntity> GetBy(Guid id); // only allowed find the entity for update or delete

    Task<bool> Add(TEntity entity);

    // Task<bool> Update(TEntity entity);
    //
    // Task<bool> Delete(TEntity entity);
}