using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Domain.AggregatesModels.BlogCategories.Dto;
using Blog.Domain.AggregatesModels.BlogCategories.Models;
using Blog.Domain.Core.Repositories;
using MediatR;
using FrameworkCore.Mapper.IQueryableUtils;

namespace Blog.Domain.AggregatesModels.BlogCategories.Queries.GetAllBlogCategories
{
    public class GetAllBlogCategoriesQueryHandler : IRequestHandler<GetAllBlogCategoriesQuery,List<BlogCategoryDto>>
    {
        private readonly IBlogEngineRepository<BlogCategoryEntity, int> _blogCategoryRepository;

        public GetAllBlogCategoriesQueryHandler(IBlogEngineRepository<BlogCategoryEntity, int> blogCategoryRepository)
        {
            _blogCategoryRepository = blogCategoryRepository;
        }

        public async Task<List<BlogCategoryDto>> Handle(GetAllBlogCategoriesQuery request, CancellationToken cancellationToken)
        {
            var blogs = await _blogCategoryRepository.GetAllAsync();
            blogs = blogs.OrderBy(x => x.Name);

            return blogs.QueryTo<BlogCategoryDto>().ToList();
        }
    }
}
