using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Application.Seedwork.RequestDto.Categories;
using Blog.Application.Seedwork.ServiceContracts;
using Blog.Domain.AggregatesModels.BlogCategories.Commands.CreateCategory;
using Blog.Domain.AggregatesModels.BlogCategories.Dto;
using Blog.Domain.AggregatesModels.BlogCategories.Queries.GetAllBlogCategories;
using Blog.Domain.Core.Commands;
using FrameworkCore.Mapper.ObjUtils;

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

        public Task<BlogCategoryDto> BetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task Create(CreateCategoryRequestDto request)
        {
            var command = request.MapTo<CreateCategoryCommand>();
            await _commandDispatcher.SendCommand(command);
        }
    }
}
