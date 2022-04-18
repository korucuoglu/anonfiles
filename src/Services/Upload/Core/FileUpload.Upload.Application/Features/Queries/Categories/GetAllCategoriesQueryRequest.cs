using AutoMapper;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Application.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Queries.Categories
{
    public class GetAllCategoriesQueryRequest : IRequest<Response<List<GetCategoryDto>>>
    {

    }
    public class GetAllCategoriesQueryRequestHandler : IRequestHandler<GetAllCategoriesQueryRequest, Response<List<GetCategoryDto>>>
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;
        private readonly ICategoryReadRepository _categoryRepository;
        public GetAllCategoriesQueryRequestHandler(ISharedIdentityService sharedIdentityService, IMapper mapper, ICategoryReadRepository categoryRepository)
        {
            _sharedIdentityService = sharedIdentityService;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<List<GetCategoryDto>>> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _categoryRepository.Where(x => x.UserId == _sharedIdentityService.GetUserId);

            return Response<List<GetCategoryDto>>.Success(await _mapper.ProjectTo<GetCategoryDto>(data).ToListAsync(), 200);

          
        }
    }
}
