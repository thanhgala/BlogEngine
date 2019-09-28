using FrameworkCore.Infrastructure.Entity.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FrameworkCore.Infrastructure.DBContext
{
    public abstract class IdentityGenericContext<TUser, TRole> : IdentityDbContext<TUser, TRole, Guid>
        where TUser : IdentityUser<Guid>
        where TRole : IdentityRole<Guid>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected IdentityGenericContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override int SaveChanges()
        {
            AddAuditData();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddAuditData();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public virtual void AddAuditData()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            var userName = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "username")?.Value ?? string.Empty;

            foreach (var item in modified)
            {
                var dateTracking = (IDateTracking) item.Entity;
                {
                    if (item.State == EntityState.Added)
                    {
                        dateTracking.DateCreated = DateTime.Now;
                    }
                    dateTracking.DateModified = DateTime.Now;
                }

                var modifiedTracking = (IModifiedTracking) item.Entity;
                {
                    if (item.State == EntityState.Added)
                    {
                        modifiedTracking.CreatedBy = userName;
                    }
                    modifiedTracking.ModifiedBy = userName;
                }
            }
        }

    }
}
