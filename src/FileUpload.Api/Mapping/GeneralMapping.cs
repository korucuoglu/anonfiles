using AutoMapper;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Data.Entity;
using FileUpload.Shared.Services;

namespace FileUpload.Api.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<AddCategoryDto, Category>().ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom<IdentityResolver<AddCategoryDto, Category>>());
            CreateMap<UpdateCategory, Category>().ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom<IdentityResolver<UpdateCategory, Category>>());
        }
    }
}