using AutoMapper;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Features.Commands.Categories.Add;
using FileUpload.Application.Features.Commands.Categories.Update;
using FileUpload.Domain.Entities;
using System.Collections.Generic;

namespace FileUpload.Application.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<AddCategoryCommand, Category>().ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom<IdentityResolver<AddCategoryCommand, Category>>());
            CreateMap<UpdateCategoryCommand, Category>().ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom<IdentityResolver<UpdateCategoryCommand, Category>>());

            CreateMap<Category, GetCategoryDto>();
        }
    }
}
