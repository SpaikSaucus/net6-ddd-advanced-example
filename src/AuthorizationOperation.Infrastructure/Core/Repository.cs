using AuthorizationOperation.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace AuthorizationOperation.Infrastructure.Core
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;

        public Repository(DbContext context)
        {
            this._context = context;
        }

        public void Add(TEntity entity)
        {
            this._context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            this._context.Set<TEntity>().AddRange(entities);
        }

        public bool Contains(ISpecification<TEntity> specification = null)
        {
            return this.Count(specification) > 0;
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Count(predicate) > 0;
        }

        public int Count(ISpecification<TEntity> specification = null)
        {
            return this.ApplySpecification(specification).Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return this._context.Set<TEntity>().Where(predicate).Count();
        }

        public IEnumerable<TEntity> Find(ISpecification<TEntity> specification = null)
        {
            return this.ApplySpecification(specification);
        }

        public TEntity FindById(int id)
        {
            return this._context.Set<TEntity>().Find(id);
        }

        public TEntity FindById(uint id)
        {
            return this._context.Set<TEntity>().Find(id);
        }

        public void Remove(TEntity entity)
        {
            this._context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            this._context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            this._context.Set<TEntity>().Attach(entity);
            this._context.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(this._context.Set<TEntity>().AsQueryable(), spec);
        }
    }
}
