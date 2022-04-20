using AutoMapper;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
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
        private readonly IUnitOfWork _unitOfWork;
        public GetAllCategoriesQueryRequestHandler(ISharedIdentityService sharedIdentityService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _sharedIdentityService = sharedIdentityService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<GetCategoryDto>>> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.CategoryReadRepository().Where(x => x.UserId == _sharedIdentityService.GetUserId, tracking: false);
            var mapperData = await _mapper.ProjectTo<GetCategoryDto>(data).ToListAsync();
            return Response<List<GetCategoryDto>>.Success(mapperData, 200);
        }
    }
}
