using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Infrastructure.EF;
using System;
using System.Collections;

namespace AuthorizationOperation.Infrastructure.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthorizationDbContext context;
        private Hashtable repositories;

        public UnitOfWork(AuthorizationDbContext context)
        {
            this.context = context;
        }

        public int Complete()
        {
            return this.context.SaveChanges();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (this.repositories == null)
                this.repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!this.repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(TEntity)), this.context);

                this.repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity>)this.repositories[type];
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
