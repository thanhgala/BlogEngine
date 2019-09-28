using FrameworkCore.Infrastructure.DAL;
using FrameworkCore.Infrastructure.Entity;

namespace Blog.Domain.Core.Repositories
{
    public interface IBlogEngineRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
        where TEntity : DomainEntity<TId>
    {

    }
}
