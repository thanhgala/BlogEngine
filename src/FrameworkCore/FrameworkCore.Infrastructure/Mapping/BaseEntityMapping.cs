using FrameworkCore.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrameworkCore.Infrastructure.Mapping
{
    public abstract class BaseEntityMapping<TEntity, TId> : ITypeConfiguration<TEntity>
        where TEntity : DomainEntity<TId>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            if (typeof(TId) == typeof(int))
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            }
            else
            {
                builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
            }

            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Ignore(x => x.RowVersion);
        }
    }
}
