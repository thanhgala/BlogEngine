using Blog.Domain.AggregatesModels.BlogCategories.Models;
using Blog.Domain.Core.Models;

namespace Blog.Domain.AggregatesModels.Blogs.Models
{
    public class BlogEntity : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public int BlogCategoryId { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public int ViewCount { get; set; }

        public virtual BlogCategoryEntity BlogCategory { get; set; }
    }
}
