using AutoMapper;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Upload.Application.Features.Commands.Categories.Add;
using FileUpload.Upload.Application.Features.Commands.Categories.Update;
using FileUpload.Upload.Domain.Entities;
using System.Collections.Generic;

namespace FileUpload.Upload.Application.Mapping
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
