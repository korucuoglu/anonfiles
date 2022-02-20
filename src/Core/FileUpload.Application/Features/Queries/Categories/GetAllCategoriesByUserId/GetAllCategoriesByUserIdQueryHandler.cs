using AutoMapper;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries
{

    public class GetAllCategoriesByUserIdQueryHandler : IRequestHandler<GetAllCategoriesByUserIdQueryRequest, List<GetCategoryDto>>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;
        public GetAllCategoriesByUserIdQueryHandler(IRepository<Category> categoryRepository, IMapper mapper, ISharedIdentityService sharedIdentityService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<List<GetCategoryDto>> Handle(GetAllCategoriesByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync(x => x.ApplicationUserId == _sharedIdentityService.GetUserId);

            return _mapper.Map<List<GetCategoryDto>>(categories);

        }
    }
}
