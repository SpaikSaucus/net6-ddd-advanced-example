using System;

namespace AuthorizationOperation.Domain.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        int Complete();
    }
}
