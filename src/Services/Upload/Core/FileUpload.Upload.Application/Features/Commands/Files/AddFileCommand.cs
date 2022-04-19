using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Upload.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Files
{
    public class AddFileCommand : IRequest<Response<AddFileDto>>
    {
        public File File { get; set; }
    }
    public class AddFileCommandValidator : AbstractValidator<AddFileCommand>
    {
        public AddFileCommandValidator()
        {
            RuleFor(x => x.File).NotNull().NotEmpty().WithMessage("Lütfen dosyayı giriniz");
        }
    }

    public class AddFileCommandHandler : IRequestHandler<AddFileCommand, Response<AddFileDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashService _hashService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public AddFileCommandHandler(IUnitOfWork unitOfWork, IHashService hashService, ISharedIdentityService sharedIdentityService)
        {
            _unitOfWork = unitOfWork;
            _hashService = hashService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<AddFileDto>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            request.File.UserId = _sharedIdentityService.GetUserId;

            bool result = await _unitOfWork.FileWriteRepository().AddFileWithSp(request.File);

            if (!result)
            {
                return Response<AddFileDto>.Fail("Dosyanın kaydedilmesi sırasında hata meydana geldi", 500);
            }

            return Response<AddFileDto>.Success($"{request.File.FileName} başarıyla kaydedildi", 200);
        }
    }
}
