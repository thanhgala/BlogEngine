using Blog.Domain.AggregatesModels.BlogCategories.Models;
using FrameworkCore.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Data.Maps
{
    public class BlogCategoryEntityMap : BaseEntityWithVersionMapping<BlogCategoryEntity, int>
    {
        public override void Configure(EntityTypeBuilder<BlogCategoryEntity> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(BlogCategoryEntity));
        }
    }
}
