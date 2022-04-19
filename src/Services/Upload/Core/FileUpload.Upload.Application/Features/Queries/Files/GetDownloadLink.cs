using FileUpload.Shared.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Queries.Files
{
    public class GetDownloadLink : IRequest<Response<NoContent>>
    {
        public string FileId { get; set; }
    }
    public class GetDownloadLinkHandler : IRequestHandler<GetDownloadLink, Response<NoContent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashService _hashService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMinioService _minioService;
        public GetDownloadLinkHandler(IUnitOfWork unitOfWork, ISharedIdentityService sharedIdentityService, IMinioService minioService, IHashService hashService)
        {
            _unitOfWork = unitOfWork;
            _sharedIdentityService = sharedIdentityService;
            _minioService = minioService;
            _hashService = hashService;
        }

        public async Task<Response<NoContent>> Handle(GetDownloadLink request, CancellationToken cancellationToken)
        {
            var fileKey = await _unitOfWork.FileReadRepository().GetFileKey(_hashService.Decode(request.FileId), _sharedIdentityService.GetUserId);

            if (string.IsNullOrEmpty(fileKey))
            {
                return Response<NoContent>.Fail("Böyle bir veri bulunamadı", 500);
            }

            var link = await _minioService.Download(fileKey);

            return Response<NoContent>.Success(message: link.Message, statusCode: 200);
        }


    }
}
