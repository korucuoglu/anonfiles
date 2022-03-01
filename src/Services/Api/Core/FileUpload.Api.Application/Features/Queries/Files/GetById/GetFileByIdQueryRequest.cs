using AutoMapper;
using FileUpload.Api.Application.Dtos.Files;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Queries.Files.GetById
{
    public class GetFileByIdQueryRequest : IRequest<Response<GetFileDto>>
    {
        public string UserId { get; set; }
        public string FileId { get; set; }
    }
    public class GetAllFilesQueryRequestHandler : IRequestHandler<GetFileByIdQueryRequest, Response<GetFileDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllFilesQueryRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<GetFileDto>> Handle(GetFileByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.GetRepository<File>().Where(x => x.UserId == request.UserId && x.Id == request.FileId);

            var mapperData = _mapper.ProjectTo<GetFileDto>(data).FirstOrDefault();

            return Response<GetFileDto>.Success(mapperData, 200);
        }

    }
}
