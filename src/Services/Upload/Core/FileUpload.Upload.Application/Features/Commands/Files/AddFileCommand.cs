using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Files
{
    public class AddFileCommand : IRequest<Response<NoContent>>
    {
        public IFormFile File { get; set; }
    }

    public class AddFileCommandHandler : IRequestHandler<AddFileCommand, Response<NoContent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMinioService _minioService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public AddFileCommandHandler(IUnitOfWork unitOfWork, ISharedIdentityService sharedIdentityService, IMinioService minioService)
        {
            _unitOfWork = unitOfWork;
            _sharedIdentityService = sharedIdentityService;
            _minioService = minioService;
        }

        public async Task<Response<NoContent>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            var fileKey = await _minioService.Upload(request.File);

            if (!fileKey.IsSuccessful)
            {
                return Response<NoContent>.Fail(fileKey.Errors, 500);
            }

            bool result = await _unitOfWork.FileWriteRepository().
                AddFileWithSp(request.File.FileName, request.File.Length, fileKey.Value, _sharedIdentityService.GetUserId);

            if (!result)
            {
                return Response<NoContent>.Fail("Dosyanın kaydedilmesi sırasında hata meydana geldi", 500);
            }

            return Response<NoContent>.Success($"{request.File.FileName} başarıyla kaydedildi", 201);
        }
    }
}
