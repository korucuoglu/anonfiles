using AutoMapper;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FileUpload.Shared.Dtos.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Categories.GetAll
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
            var data = _unitOfWork.ReadRepository<Category>().Where(x => x.ApplicationUserId == request.UserId, false);

            var mapperData = await _mapper.ProjectTo<GetCategoryDto>(data).ToListAsync();

            return Response<List<GetCategoryDto>>.Success(mapperData, 200);

        }
    }
}
