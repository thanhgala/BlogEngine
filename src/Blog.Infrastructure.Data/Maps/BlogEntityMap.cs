using Blog.Domain.AggregatesModels.Blogs.Models;
using FrameworkCore.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Data.Maps
{
    public class BlogEntityMap : BaseEntityWithVersionMapping<BlogEntity, int>
    {
        public override void Configure(EntityTypeBuilder<BlogEntity> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(BlogEntity));
        }
    }
}
