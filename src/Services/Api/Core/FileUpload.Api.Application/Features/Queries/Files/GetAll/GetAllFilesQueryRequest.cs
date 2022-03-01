using AutoMapper;
using FileUpload.Api.Application.Dtos.Files;
using FileUpload.Api.Application.Dtos.Files.Pager;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Queries.Files.GetAll
{
    public class GetAllFilesQueryRequest : IRequest<Response<FilesPagerViewModel>>
    {
        public FileFilterModel FilterModel { get; set; }
        public string UserId { get; set; }
    }
    public class GetAllFilesQueryRequestHandler : IRequestHandler<GetAllFilesQueryRequest, Response<FilesPagerViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllFilesQueryRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<FilesPagerViewModel>> Handle(GetAllFilesQueryRequest request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<File>();

            if (await repository.Any(x => x.UserId == request.UserId))
            {
                return await Helper.Filter.FilterFile(repository.Where(x => x.UserId == request.UserId), request.FilterModel, _mapper);
            }

            return Response<FilesPagerViewModel>.Success(new FilesPagerViewModel(), 200);
        }
    }
}
