using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Seedwork.RequestDto.Categories
{
    public class CreateCategoryRequestDto
    {
        public string Name { get; set; }

        public string Slug { get; set; }
    }
}
