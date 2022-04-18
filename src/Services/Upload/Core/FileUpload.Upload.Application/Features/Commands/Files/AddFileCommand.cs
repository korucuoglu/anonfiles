using AutoMapper;
using FileUpload.Shared.Dtos.Files;
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
        private readonly IMapper _mapper;
        private readonly ISharedIdentityService _sharedIdentityService;

        public AddFileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ISharedIdentityService sharedIdentityService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<AddFileDto>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            request.File.UserId = _sharedIdentityService.GetUserId;

            var userInfo = await _unitOfWork.UserInfoReadRepository().FirstOrDefaultAsync(x => x.Id == request.File.UserId);

            userInfo.UsedSpace += request.File.Size;

            _unitOfWork.UserInfoWriteRepository().Update(userInfo);

            var data = await _unitOfWork.FileWriteRepository().AddAsync(request.File);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<AddFileDto>.Fail("Dosyanın kaydedilmesi sırasında hata meydana geldi", 500);
            }

            return Response<AddFileDto>.Success(_mapper.Map<AddFileDto>(data), 201);
        }
    }
}
