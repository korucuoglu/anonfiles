using FileUpload.Upload.Application.Helper;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Pager;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FileUpload.Upload.Application.Interfaces.Services;

namespace FileUpload.Upload.Application.Features.Queries.Files
{
    public class GetAllFilesQueryRequest : IRequest<Response<FilesPagerViewModel>>
    {
        public FileFilterModel FilterModel { get; set; }
    }
    public class GetAllFilesQueryRequestHandler : IRequestHandler<GetAllFilesQueryRequest, Response<FilesPagerViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISharedIdentityService _sharedIdentityService;

        public GetAllFilesQueryRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ISharedIdentityService sharedIdentityService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<FilesPagerViewModel>> Handle(GetAllFilesQueryRequest request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.ReadRepository<File>();

            if (repository.Any(x => x.ApplicationUserId == _sharedIdentityService.GetUserId))
            {
                return await Filter.FilterFile(
                    repository.Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId, tracking: false), request.FilterModel, _mapper);
            }

            return Response<FilesPagerViewModel>.Success(new FilesPagerViewModel(), 200);
        }
    }
}
