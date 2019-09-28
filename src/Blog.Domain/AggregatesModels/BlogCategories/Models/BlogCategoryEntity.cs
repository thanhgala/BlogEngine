using System.Collections.Generic;
using Blog.Domain.AggregatesModels.Blogs.Models;
using Blog.Domain.Core.Models;

namespace Blog.Domain.AggregatesModels.BlogCategories.Models
{
    public class BlogCategoryEntity : BaseEntity<int>
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public virtual ICollection<BlogEntity> Blogs { get; set; }
    }
}
