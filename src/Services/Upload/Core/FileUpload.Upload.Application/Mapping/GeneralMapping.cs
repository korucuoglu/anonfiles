﻿using AutoMapper;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Services;
using FileUpload.Upload.Application.Features.Commands.Categories;
using FileUpload.Upload.Domain.Entities;

namespace FileUpload.Upload.Application.Mapping
{
    public class GeneralMapping : Profile
    {
        private readonly IHashService _hashService;

        public GeneralMapping()
        {
            _hashService = new HashService();

            #region Category

            CreateMap<AddCategoryCommand, Category>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom<IdentityResolver<AddCategoryCommand, Category>>());


            CreateMap<UpdateCategoryCommand, Category>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom<IdentityResolver<UpdateCategoryCommand, Category>>())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashService.Decode(src.Id)));

            CreateMap<Category, GetCategoryDto>().
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashService.Encode(src.Id)));

            CreateMap<GetCategoryDto, Category>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom<IdentityResolver<GetCategoryDto, Category>>())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashService.Decode(src.Id)));

            #endregion

            #region File

            CreateMap<File, GetFileDto>().
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => _hashService.Encode(src.Id)));

            CreateMap<File, AddFileDto>().
                ForMember(dest => dest.FileId, opt => opt.MapFrom(src => _hashService.Encode(src.Id)));

            #endregion
        }
    }
}
