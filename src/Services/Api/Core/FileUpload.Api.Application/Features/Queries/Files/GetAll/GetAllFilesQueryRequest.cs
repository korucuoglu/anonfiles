using AutoMapper;
using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Dtos.Files.Pager;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Files.GetAll
{
    public class GetAllFilesQueryRequest : IRequest<Response<FilesPagerViewModel>>
    {
        public FileFilterModel FilterModel { get; set; }
        public Guid UserId { get; set; }
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
            var repository = _unitOfWork.ReadRepository<File>();

            if (repository.Any(x => x.ApplicationUserId == request.UserId))
            {
                return await Helper.Filter.FilterFile(repository.Where(x => x.ApplicationUserId == request.UserId), request.FilterModel, _mapper);
            }

            return Response<FilesPagerViewModel>.Success(new FilesPagerViewModel(), 200);
        }
    }
}
