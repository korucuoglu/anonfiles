﻿using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Dtos.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Interfaces.Services;
using AutoMapper;
using FileUpload.Upload.Application.Interfaces.Repositories;

namespace FileUpload.Upload.Application.Features.Queries.Categories
{
    public class GetAllCategoriesQueryRequest : IRequest<Response<List<GetCategoryDto>>>
    {

    }
    public class GetAllCategoriesQueryRequestHandler : IRequestHandler<GetAllCategoriesQueryRequest, Response<List<GetCategoryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        public GetAllCategoriesQueryRequestHandler(IUnitOfWork unitOfWork, ISharedIdentityService sharedIdentityService, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _sharedIdentityService = sharedIdentityService;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<List<GetCategoryDto>>> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _categoryRepository.GetAll(_sharedIdentityService.GetUserId);

            var mapperData = _mapper.Map<List<GetCategoryDto>>(data);

            return Response<List<GetCategoryDto>>.Success(mapperData, 200);



            // var data = _unitOfWork.ReadRepository<Category>().Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId, false);



            //var mapperData = await _mapper.ProjectTo<GetCategoryDto>(data).ToListAsync();

            //return Response<List<GetCategoryDto>>.Success(mapperData, 200);
        }
    }
}
