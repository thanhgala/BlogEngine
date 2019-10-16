using System;
using System.Collections.Generic;
using System.Text;
using Blog.Domain.AggregatesModels.BlogCategories.Models;
using Blog.Domain.Core.Events;

namespace Blog.Domain.AggregatesModels.BlogCategories.Events
{
    public class AddCagtegoryEvent : DomainEvent
    {
        public BlogCategoryEntity Category { get; set; }

        public override void Flatten()
        {
            Args.Add("Name",Category.Name);
            Args.Add("Slug", Category.Slug);
        }
    }
}
