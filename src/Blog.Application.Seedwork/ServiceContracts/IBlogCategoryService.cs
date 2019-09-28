using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain.AggregatesModels.BlogCategories.Dto;
namespace Blog.Application.Seedwork.ServiceContracts
{
    public interface IBlogCategoryService
    {
        Task<List<BlogCategoryDto>> GetAll();
    }
}
