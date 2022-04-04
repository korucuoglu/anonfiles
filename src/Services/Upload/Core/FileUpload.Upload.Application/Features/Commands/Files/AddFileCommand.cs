using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Upload.Application.Mapping;

namespace FileUpload.Upload.Application.Features.Commands.Files.Add
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

        public AddFileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<AddFileDto>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {

            var userInfo = await _unitOfWork.ReadRepository<UserInfo>().FirstOrDefaultAsync(x => x.ApplicationUserId == request.File.ApplicationUserId);

            userInfo.UsedSpace += request.File.Size;

            _unitOfWork.WriteRepository<UserInfo>().Update(userInfo);

            var data = await _unitOfWork.WriteRepository<File>().AddAsync(request.File);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<AddFileDto>.Fail("Dosyanın kaydedilmesi sırasında hata meydana geldi", 500);
            }

            var mapperData = ObjectMapper.Mapper.Map<AddFileDto>(data);

            return Response<AddFileDto>.Success(mapperData, 200);
        }
    }
}
