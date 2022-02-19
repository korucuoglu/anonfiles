using AutoMapper;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Domain.Entities;
using System.Collections.Generic;

namespace FileUpload.Application.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<AddCategoryDto, Category>().ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom<IdentityResolver<AddCategoryDto, Category>>());
            CreateMap<UpdateCategory, Category>().ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom<IdentityResolver<UpdateCategory, Category>>());
            
            CreateMap<Category, GetCategoryDto>();
            CreateMap<List<Category>, List<GetCategoryDto>>();
        }
    }
}
