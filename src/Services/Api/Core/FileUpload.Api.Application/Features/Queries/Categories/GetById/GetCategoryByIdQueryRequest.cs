using AutoMapper;
using FileUpload.Api.Application.Dtos.Categories;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Queries.Categories.GetById
{
    public class GetCategoryByIdQueryRequest: IRequest<Response<GetCategoryDto>>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
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
            var data = _unitOfWork.GetRepository<Category>().Where(x => x.UserId == request.UserId && x.Id == request.Id);

            var mapperData = _mapper.ProjectTo<GetCategoryDto>(data).FirstOrDefault();

            return Response<GetCategoryDto>.Success(mapperData, 200);
        }
    }
}
