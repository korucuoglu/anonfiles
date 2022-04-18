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
        public int FileId { get; set; }
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
        private readonly ISharedIdentityService _sharedIdentityService;

        public DeleteFileCommandHandler(IUnitOfWork unitOfWork, ISharedIdentityService sharedIdentityService)
        {
            _unitOfWork = unitOfWork;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<NoContent>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            //var fileReadRepository = _unitOfWork.ReadRepository<File>();
            //var fileWriteRepository = _unitOfWork.WriteRepository<File>();

            //var file = await fileReadRepository.FirstOrDefaultAsync(
            //    x => x.ApplicationUserId == _sharedIdentityService.GetUserId && x.Id == request.FileId);

            //var userInfo = await _unitOfWork.ReadRepository<UserInfo>().FirstOrDefaultAsync(
            //    x => x.ApplicationUserId == _sharedIdentityService.GetUserId);

            //userInfo.UsedSpace -= file.Size;

            //_unitOfWork.WriteRepository<UserInfo>().Update(userInfo);

            //fileWriteRepository.Remove(file);

            //bool result = await _unitOfWork.SaveChangesAsync() > 0;

            //if (!result)
            //{
            //    return Response<NoContent>.Fail("Veri silme sırasında hata meydana geldi", 500);
            //}

            return Response<NoContent>.Success(204);


        }
    }
}
