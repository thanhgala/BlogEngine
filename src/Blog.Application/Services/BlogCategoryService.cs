using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Application.Seedwork.ServiceContracts;
using Blog.Domain.AggregatesModels.BlogCategories.Dto;
using Blog.Domain.AggregatesModels.BlogCategories.Queries.GetAllBlogCategories;
using Blog.Domain.Core.Commands;

namespace Blog.Application.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public BlogCategoryService(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task<List<BlogCategoryDto>> GetAll()
        {
            var data = await _commandDispatcher.SendQuery(new GetAllBlogCategoriesQuery());

            return data;
        }
    }
}
