using Blog.Domain.Core.Repositories;
using Blog.Infrastructure.Data.Context;
using FrameworkCore.Infrastructure.DAL;
using FrameworkCore.Infrastructure.Entity;

namespace Blog.Infrastructure.Data.Repositories
{
    public class BlogEngineRepository<TEntity, TId> : GenericRepository<TEntity, TId, BlogDbContext>, IBlogEngineRepository<TEntity, TId>
        where TEntity : DomainEntity<TId>
    {
        public BlogEngineRepository(BlogDbContext context) : base(context)
        {

        }

    }
}
