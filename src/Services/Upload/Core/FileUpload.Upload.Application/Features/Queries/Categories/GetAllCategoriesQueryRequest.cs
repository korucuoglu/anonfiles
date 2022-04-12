using FileUpload.Upload.Application.Interfaces.UnitOfWork;
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
        public GetAllCategoriesQueryRequestHandler(IUnitOfWork unitOfWork, ISharedIdentityService sharedIdentityService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _sharedIdentityService = sharedIdentityService;
            _mapper = mapper;
        }

        public async Task<Response<List<GetCategoryDto>>> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.ReadRepository<Category>().Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId, false);

            var mapperData = await _mapper.ProjectTo<GetCategoryDto>(data).ToListAsync();

            return Response<List<GetCategoryDto>>.Success(mapperData, 200);
        }
    }
}
