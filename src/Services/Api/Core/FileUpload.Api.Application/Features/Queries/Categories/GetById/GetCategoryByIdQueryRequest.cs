using AutoMapper;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FileUpload.Shared.Dtos.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Categories.GetById
{
    public class GetCategoryByIdQueryRequest: IRequest<Response<GetCategoryDto>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetCategoryByIdQueryRequestHandler : IRequestHandler<GetCategoryByIdQueryRequest, Response<GetCategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdQueryRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<GetCategoryDto>> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.ReadRepository<Category>().Where(x => x.ApplicationUserId == request.UserId && x.Id == request.Id, false);

            var mapperData = await _mapper.ProjectTo<GetCategoryDto>(data).FirstOrDefaultAsync();

            return Response<GetCategoryDto>.Success(mapperData, 200);
        }
    }
}
