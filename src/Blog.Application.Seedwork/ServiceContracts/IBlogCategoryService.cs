using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Application.Seedwork.RequestDto.Categories;
using Blog.Domain.AggregatesModels.BlogCategories.Dto;
namespace Blog.Application.Seedwork.ServiceContracts
{
    public interface IBlogCategoryService
    {
        Task<List<BlogCategoryDto>> GetAll();

        Task<BlogCategoryDto> BetById(int id);

        Task Create(CreateCategoryRequestDto request);
    }
}
