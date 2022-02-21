using AutoMapper;
using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Files.GetById
{
    public class GetFileByIdQueryRequest : IRequest<Response<FileDto>>
    {
        public Guid UserId { get; set; }
        public Guid FileId { get; set; }
    }
    public class GetAllFilesQueryRequestHandler : IRequestHandler<GetFileByIdQueryRequest, Response<FileDto>>
    {
        private readonly IRepository<Domain.Entities.File> _repository;
        private readonly IMapper _mapper;
        public GetAllFilesQueryRequestHandler(IRepository<Domain.Entities.File> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<FileDto>> Handle(GetFileByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _repository.Where(x => x.ApplicationUserId == request.UserId && x.Id == request.FileId);

            var mapperData = await _mapper.ProjectTo<FileDto>(data).FirstOrDefaultAsync();

            return Response<FileDto>.Success(mapperData, 200);
        }

    }
}
