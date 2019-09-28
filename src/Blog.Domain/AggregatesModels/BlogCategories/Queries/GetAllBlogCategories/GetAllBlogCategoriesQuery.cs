using System.Collections.Generic;
using Blog.Domain.AggregatesModels.BlogCategories.Dto;
using MediatR;

namespace Blog.Domain.AggregatesModels.BlogCategories.Queries.GetAllBlogCategories
{
    public class GetAllBlogCategoriesQuery : IRequest<List<BlogCategoryDto>>
    {

    }
}
