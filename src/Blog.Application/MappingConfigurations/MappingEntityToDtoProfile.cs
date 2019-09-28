using AutoMapper;
using Blog.Domain.AggregatesModels.BlogCategories.Dto;
using Blog.Domain.AggregatesModels.BlogCategories.Models;
using FrameworkCore.Mapper.MappingExpression;

namespace Blog.Application.MappingConfigurations
{
    public class MappingEntityToDtoProfile : Profile
    {
        public MappingEntityToDtoProfile()
        {
            CreateMap<BlogCategoryEntity, BlogCategoryDto>()
                .IgnoreAllNonExisting();
        }
    }
}
