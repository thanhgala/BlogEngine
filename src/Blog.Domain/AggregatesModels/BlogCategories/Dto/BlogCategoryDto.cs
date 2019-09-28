using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.AggregatesModels.BlogCategories.Dto
{
    public class BlogCategoryDto
    {
        public string Name { get; set; }

        public string Slug { get; set; }
    }
}
