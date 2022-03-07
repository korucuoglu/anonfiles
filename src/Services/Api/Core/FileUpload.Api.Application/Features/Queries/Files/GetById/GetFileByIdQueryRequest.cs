using AutoMapper;
using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Files.GetById
{
    public class GetFileByIdQueryRequest : IRequest<Response<GetFileDto>>
    {
        public Guid UserId { get; set; }
        public Guid FileId { get; set; }
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
            var data = _unitOfWork.ReadRepository<File>().Where(x => x.ApplicationUserId == request.UserId && x.Id == request.FileId);

            var mapperData = await _mapper.ProjectTo<GetFileDto>(data).FirstOrDefaultAsync();

            return Response<GetFileDto>.Success(mapperData, 200);
        }

    }
}
