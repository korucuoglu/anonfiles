using AutoMapper;
using FileUpload.Api.Application.Dtos.Categories;
using FileUpload.Api.Application.Dtos.Files;
using FileUpload.Api.Application.Features.Commands.Categories.Add;
using FileUpload.Api.Application.Features.Commands.Categories.Update;
using FileUpload.Api.Domain.Entities;
using System.Collections.Generic;

namespace FileUpload.Api.Application.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            #region Category

            CreateMap<AddCategoryCommand, Category>();
            CreateMap<UpdateCategoryCommand, Category>();


            CreateMap<Category, GetCategoryDto>().ReverseMap();

            CreateMap<IEnumerable<Category>, GetCategoryDto>().ReverseMap();

            #endregion

            #region File

              CreateMap<File, GetFileDto>();

            #endregion
        }
    }
}
