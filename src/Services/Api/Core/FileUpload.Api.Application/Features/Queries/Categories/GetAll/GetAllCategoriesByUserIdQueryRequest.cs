using AutoMapper;
using FileUpload.Api.Application.Dtos.Categories;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Queries.Categories.GetAll
{
    public class GetAllCategoriesQueryRequest : IRequest<Response<List<GetCategoryDto>>>
    {
        public Guid UserId { get; set; }
    }
    public class GetAllCategoriesQueryRequestHandler : IRequestHandler<GetAllCategoriesQueryRequest, Response<List<GetCategoryDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetAllCategoriesQueryRequestHandler( IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<GetCategoryDto>>> Handle(GetAllCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.GetRepository<Category>().Where(x => x.ApplicationUserId == request.UserId);

            var mapperData = _mapper.ProjectTo<GetCategoryDto>(data).ToList();

            return Response<List<GetCategoryDto>>.Success(mapperData, 200);

        }
    }
}
