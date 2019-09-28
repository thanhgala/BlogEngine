using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrameworkCore.Infrastructure.Mapping
{
    public interface ITypeConfiguration<TEntity> where TEntity : class
    {
        void Configure(EntityTypeBuilder<TEntity> builder);
    }
}
