using AutoMapper;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Services;
using FileUpload.Upload.Application.Features.Commands.Categories.Add;
using FileUpload.Upload.Application.Features.Commands.Categories.Update;
using FileUpload.Upload.Domain.Entities;
using System.Collections.Generic;

namespace FileUpload.Upload.Application.Mapping
{
    public class GeneralMapping : Profile
    {

        private readonly IHashService _hashService;


        public GeneralMapping()
        {
            _hashService = new HashService();

            #region Category

            CreateMap<AddCategoryCommand, Category>();

            CreateMap<UpdateCategoryCommand, Category>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashService.Decode(src.Id)));

            CreateMap<Category, GetCategoryDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashService.Encode(src.Id)));

            CreateMap<GetCategoryDto, Category>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashService.Decode(src.Id)));

            CreateMap<IEnumerable<Category>, GetCategoryDto>().ReverseMap();

            #endregion

            #region File

              CreateMap<File, GetFileDto>();

            #endregion
        }
    }
}
