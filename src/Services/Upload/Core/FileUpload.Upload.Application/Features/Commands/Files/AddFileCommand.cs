using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Shared.Dtos.Files;
using AutoMapper;
using FileUpload.Upload.Application.Interfaces.Services;

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
            request.File.ApplicationUserId = _sharedIdentityService.GetUserId;

            var userInfo = await _unitOfWork.ReadRepository<UserInfo>().FirstOrDefaultAsync(x => x.ApplicationUserId == request.File.ApplicationUserId);

            userInfo.UsedSpace += request.File.Size;

            _unitOfWork.WriteRepository<UserInfo>().Update(userInfo);

            var data = await _unitOfWork.WriteRepository<File>().AddAsync(request.File);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<AddFileDto>.Fail("Dosyanın kaydedilmesi sırasında hata meydana geldi", 500);
            }

            var mapperData = _mapper.Map<AddFileDto>(data);

            return Response<AddFileDto>.Success(mapperData, 201);
        }
    }
}
