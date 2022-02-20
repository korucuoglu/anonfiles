using AutoMapper;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Categories.GetAll
{
    public class GetAllCategoriesQueryRequest : IRequest<Response<List<GetCategoryDto>>>
    {

    }
    public class GetAllCategoriesQueryRequestHandler : IRequestHandler<GetAllCategoriesQueryRequest, Response<List<GetCategoryDto>>>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;
        public GetAllCategoriesQueryRequestHandler(IRepository<Category> categoryRepository, IMapper mapper, ISharedIdentityService sharedIdentityService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<List<GetCategoryDto>>> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {

            var data = await _categoryRepository.GetAllAsync(x => x.ApplicationUserId == _sharedIdentityService.GetUserId);

            var mapperData = _mapper.Map<List<GetCategoryDto>>(data);

            return Response<List<GetCategoryDto>>.Success(mapperData, 200);

        }
    }
}
