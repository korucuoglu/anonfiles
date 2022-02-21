using AutoMapper;
using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Application.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Files.GetAll
{
    public class GetAllFilesQueryRequest : IRequest<Response<MyFilesViewModel>>
    {
        public FileFilterModel FilterModel { get; set; }
        public Guid UserId { get; set; }
    }
    public class GetAllFilesQueryRequestHandler : IRequestHandler<GetAllFilesQueryRequest, Response<MyFilesViewModel>>
    {
        private readonly IRepository<Domain.Entities.File> _repository;
        public GetAllFilesQueryRequestHandler(IRepository<Domain.Entities.File> repository)
        {
            _repository = repository;
        }

        public async Task<Response<MyFilesViewModel>> Handle(GetAllFilesQueryRequest request, CancellationToken cancellationToken)
        {
            if (_repository.Any(x => x.ApplicationUserId == request.UserId))
            {
                return await Helper.Filter.FilterFile(_repository.Where(x => x.ApplicationUserId == request.UserId), request.FilterModel);
            }

            return Response<MyFilesViewModel>.Success(new MyFilesViewModel(), 200);
        }
    }
}
