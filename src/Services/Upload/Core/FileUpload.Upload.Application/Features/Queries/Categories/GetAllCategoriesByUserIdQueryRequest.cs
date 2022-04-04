using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Dtos.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Mapping;

namespace FileUpload.Upload.Application.Features.Queries.Categories
{
    public class GetAllCategoriesQueryRequest : IRequest<Response<List<GetCategoryDto>>>
    {
        public int UserId { get; set; }
    }
    public class GetAllCategoriesQueryRequestHandler : IRequestHandler<GetAllCategoriesQueryRequest, Response<List<GetCategoryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllCategoriesQueryRequestHandler( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<GetCategoryDto>>> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.ReadRepository<Category>().Where(x => x.ApplicationUserId == request.UserId, false);

            var mapperData = await ObjectMapper.Mapper.ProjectTo<GetCategoryDto>(data).ToListAsync();

            return Response<List<GetCategoryDto>>.Success(mapperData, 200);

        }
    }
}
