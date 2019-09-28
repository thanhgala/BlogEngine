using FrameworkCore.Infrastructure.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FrameworkCore.Infrastructure.DAL
{
    public interface IGenericRepository<TEntity, TId> where TEntity : DomainEntity<TId>
    {
        void Add(TEntity entity);

        Task<TEntity> AddAsync(TEntity entity);

        void Update(TEntity entity, Expression<Func<TEntity, bool>> criteria);

        Task<TEntity> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> criteria);

        void Delete(TEntity entity);

        void MultipleDelete(Expression<Func<TEntity, bool>> criteria);

        int Count();

        Task<int> CountAsync();

        TEntity FindOne(Expression<Func<TEntity, bool>> match);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> match);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        Task<IQueryable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        Task<IQueryable<TEntity>> GetAllAsync();

        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IQueryable<TEntity>> QueryAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<PaginationSet<TEntity>> PaginateAsync(
            int pageIndex, int pageSize, int maxPage,
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        void Dispose();
    }
}
