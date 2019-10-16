using AutoMapper;
using Blog.Application.Seedwork.RequestDto.Categories;
using Blog.Domain.AggregatesModels.BlogCategories.Commands.CreateCategory;
using FrameworkCore.Mapper.MappingExpression;

namespace Blog.Application.MappingConfigurations
{
    public class MappingViewModelToCommandProfile : Profile
    {
        public MappingViewModelToCommandProfile()
        {
            CreateMap<CreateCategoryRequestDto, CreateCategoryCommand>()
                .IgnoreAllNonExisting();
        }
    }
}
