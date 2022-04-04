using AutoMapper;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Dtos.Files;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Mapping;

namespace FileUpload.Upload.Application.Features.Queries.Files.GetById
{
    public class GetFileByIdQueryRequest : IRequest<Response<GetFileDto>>
    {
        public int UserId { get; set; }
        public int FileId { get; set; }
    }
    public class GetAllFilesQueryRequestHandler : IRequestHandler<GetFileByIdQueryRequest, Response<GetFileDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllFilesQueryRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<GetFileDto>> Handle(GetFileByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.ReadRepository<File>().Where(x => x.ApplicationUserId == request.UserId && x.Id == request.FileId, tracking: false);

            var mapperData = await ObjectMapper.Mapper.ProjectTo<GetFileDto>(data).FirstOrDefaultAsync();

            return Response<GetFileDto>.Success(mapperData, 200);
        }

    }
}
