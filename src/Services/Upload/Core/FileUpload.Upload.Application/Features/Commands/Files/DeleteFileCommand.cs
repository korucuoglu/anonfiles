using FileUpload.Shared.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Files
{
    public class DeleteFileCommand : IRequest<Response<NoContent>>
    {
        public string FileId { get; set; }
    }
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(x => x.FileId).NotNull().NotEmpty().WithMessage("FileId boş bırakılamaz");
        }
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Response<NoContent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashService _hashService;
        private readonly IMinioService _minioService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DeleteFileCommandHandler(IUnitOfWork unitOfWork, ISharedIdentityService sharedIdentityService, IMinioService minioService, IHashService hashService)
        {
            _unitOfWork = unitOfWork;
            _sharedIdentityService = sharedIdentityService;
            _minioService = minioService;
            _hashService = hashService;
        }

        public async Task<Response<NoContent>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var fileKey = await _unitOfWork.FileReadRepository().
                GetFileKey(_hashService.Decode(request.FileId), _sharedIdentityService.GetUserId);

            if (string.IsNullOrEmpty(fileKey))
            {
                return Response<NoContent>.Fail("Böyle bir dosya bulunamadı", 500);
            }

            var minioResult = await _minioService.Remove(fileKey);

            if (!minioResult.IsSuccessful)
            {
                return minioResult;
            }

            bool result = await _unitOfWork.FileWriteRepository().
                DeleteFileWithSp(_hashService.Decode(request.FileId), _sharedIdentityService.GetUserId);

            if (!result)
            {
                return Response<NoContent>.Fail("Silme sırasında hata meydana geldi", 500);
            }

            return Response<NoContent>.Success(204);
        }
    }
}
