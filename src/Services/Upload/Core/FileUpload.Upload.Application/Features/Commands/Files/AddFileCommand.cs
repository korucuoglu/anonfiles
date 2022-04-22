using FileUpload.Shared.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Files
{
    public class AddFileCommand : IRequest<Response<string>>
    {
        public IFormFile File { get; set; }
    }

    public class AddFileCommandValidator : AbstractValidator<AddFileCommand>
    {
        public AddFileCommandValidator()
        {
            RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("Dolu bir dosya giriniz");
        }
    }

    public class AddFileCommandHandler : IRequestHandler<AddFileCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashService _hashService;
        private readonly IMinioService _minioService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public AddFileCommandHandler(IUnitOfWork unitOfWork, ISharedIdentityService sharedIdentityService, IMinioService minioService, IHashService hashService)
        {
            _unitOfWork = unitOfWork;
            _sharedIdentityService = sharedIdentityService;
            _minioService = minioService;
            _hashService = hashService;
        }

        public async Task<Response<string>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            var fileKey = await _minioService.Upload(request.File);

            if (!fileKey.IsSuccessful)
            {
                return fileKey;
            }

            int fileId = await _unitOfWork.FileWriteRepository().
                AddFileWithSp(request.File.FileName, request.File.Length, fileKey.Value, _sharedIdentityService.GetUserId);

            if (fileId <= 0)
            {
                return Response<string>.Fail("Dosyanın kaydedilmesi sırasında hata meydana geldi", 500);
            }

            return Response<string>.Success(data: $"{request.File.FileName} başarıyla kaydedildi", 201);
        }
    }
}
