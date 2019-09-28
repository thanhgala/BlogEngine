using FrameworkCore.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrameworkCore.Infrastructure.Mapping
{
    public class BaseEntityWithVersionMapping<TEntity, TId> : ITypeConfiguration<TEntity>
        where TEntity : DomainEntity<TId>
        where TId : struct
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

            builder.Property(x => x.RowVersion)
                .IsRequired()
                .IsConcurrencyToken();
        }
    }
}
