using AutoMapper;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Queries.Files
{
    public class GetFileByIdQueryRequest : IRequest<Response<GetFileDto>>
    {
        public string FileId { get; set; }
    }
    public class GetFileByIdQueryRequestHandler : IRequestHandler<GetFileByIdQueryRequest, Response<GetFileDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashService _hashService;
        private readonly IMapper _mapper;
        private readonly ISharedIdentityService _sharedIdentityService;
        public GetFileByIdQueryRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ISharedIdentityService sharedIdentityService, IHashService hashService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sharedIdentityService = sharedIdentityService;
            _hashService = hashService;
        }

        public async Task<Response<GetFileDto>> Handle(GetFileByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.FileReadRepository().Where(
                x => x.UserId == _sharedIdentityService.GetUserId && x.Id == _hashService.Decode(request.FileId), tracking: false);

            var mapperData = await _mapper.ProjectTo<GetFileDto>(data).FirstOrDefaultAsync();

            return Response<GetFileDto>.Success(mapperData, 200);
        }

    }
}
