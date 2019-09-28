using FrameworkCore.Infrastructure.Entity;
using FrameworkCore.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FrameworkCore.Infrastructure.Common;
using FrameworkCore.Infrastructure.Entity.Audit;

namespace FrameworkCore.Infrastructure.DAL
{
    public abstract class GenericRepository<TEntity, TId, TContext> : IGenericRepository<TEntity, TId>
        where TContext : DbContext
        where TEntity : DomainEntity<TId>
    {

        protected DbContext Context { get; }

        protected DbSet<TEntity> DbSet { get; }

        #region Constructure

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity, TId, TContext}"/> class.
        /// </summary>
        protected GenericRepository(TContext context)
        {
            Context = context ?? throw new ArgumentNullException($"entitiesContext");
            DbSet = Context.Set<TEntity>();
        }
        #endregion

        private IQueryable<TEntity> GetQuery()
        {
            return DbSet.AsNoTracking();
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public virtual void Update(TEntity entity, Expression<Func<TEntity, bool>> criteria)
        {
            Context.Entry(FindOne(criteria)).CurrentValues.SetValues(entity);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> criteria)
        {
            var exist = await FindOneAsync(criteria);

            Context.Entry(exist).CurrentValues.SetValues(entity);
            return exist;
        }

        public virtual async Task UpdateAsync(TId id, TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            var dbEntity = await DbSet.AsNoTracking().SingleAsync(p => p.Id.Equals(id));
            var databaseEntry = Context.Entry(dbEntity);
            var inputEntry = Context.Entry(entity);
            if (updatedProperties.Any())
            {
                foreach (var property in updatedProperties)
                {
                    databaseEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                //no items mentioned, so find out the updated entries
                var dateProperties = typeof(IDateTracking).GetPublicProperties().Select(x => x.Name);
                var modifyProperties = typeof(IModifiedTracking).GetPublicProperties().Select(x => x.Name);
                var domainProperties = typeof(DomainEntity<TId>).GetPublicProperties().Select(x => x.Name);

                var allProperties = databaseEntry.Metadata.GetProperties()
                    .Where(x => !dateProperties.Contains(x.Name))
                    .Where(x => !modifyProperties.Contains(x.Name))
                    .Where(x => !domainProperties.Contains(x.Name));

                foreach (var property in allProperties)
                {
                    var proposedValue = inputEntry.Property(property.Name).CurrentValue;
                    var originalValue = databaseEntry.Property(property.Name).OriginalValue;

                    if ((proposedValue == null || proposedValue.Equals(originalValue)) &&
                        (!property.PropertyInfo.PropertyType.IsGenericType ||
                         property.PropertyInfo.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)))
                    {
                        continue;
                    }

                    databaseEntry.Property(property.Name).IsModified = true;
                    databaseEntry.Property(property.Name).CurrentValue = proposedValue;
                }
            }
            DbSet.Update(dbEntity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void MultipleDelete(Expression<Func<TEntity, bool>> criteria)
        {
            var entities = GetQuery().Where(criteria);

            DbSet.RemoveRange(entities);
        }

        public virtual int Count()
        {
            return GetQuery().Count();
        }

        public virtual async Task<int> CountAsync()
        {
            return await GetQuery().CountAsync();
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> match)
        {
            return GetQuery().FirstOrDefault(match);
        }

        public virtual async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> match)
        {
            return await GetQuery().FirstOrDefaultAsync(match);
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQuery().Where(predicate);
        }

        public virtual async Task<IQueryable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(GetQuery().Where(predicate));
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return GetQuery();
        }

        public virtual async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(GetQuery());
        }

        public virtual IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var items = GetQuery();
            return includeProperties == null
                ? items
                : includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        }

        public virtual async Task<IQueryable<TEntity>> QueryAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var items = GetQuery();
            if (includeProperties != null)
            {
                items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
            }
            return await Task.FromResult(items);
        }

        public virtual async Task<PaginationSet<TEntity>> PaginateAsync(int pageIndex, int pageSize, int maxPage,
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = await QueryAsync(includeProperties);

            query = (predicate == null) ? query : query.Where(predicate);

            var data = query.ToPaginatedList(pageIndex, pageSize, maxPage);

            return await Task.FromResult(data);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
