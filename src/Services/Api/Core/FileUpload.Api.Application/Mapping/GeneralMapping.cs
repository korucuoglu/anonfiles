using AutoMapper;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Dtos.Files;
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
            #region Category

            CreateMap<AddCategoryCommand, Category>();
            CreateMap<UpdateCategoryCommand, Category>();


            CreateMap<Category, GetCategoryDto>().ReverseMap();

            #endregion

            #region File

            CreateMap<File, GetFileDto>();

            #endregion
        }
    }
}
