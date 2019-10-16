using System;
using System.Collections.Generic;
using System.Text;
using Blog.Domain.Core.Commands;

namespace Blog.Domain.AggregatesModels.BlogCategories.Commands
{
    public abstract class CategoryCommand : Command
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }
    }
}
