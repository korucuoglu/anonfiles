using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Pager;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Mapping;

namespace FileUpload.Upload.Application.Features.Commands.Files
{
    public class DeleteFileCommand : IRequest<Response<NoContent>>
    {
        public int FileId { get; set; }
        public int UserId { get; set; }
    }
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(x => x.FileId).NotNull().NotEmpty().WithMessage("FileId boş bırakılamaz");
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("UserId boş bırakılamaz");
        }
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Response<NoContent>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<NoContent>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var fileReadRepository = _unitOfWork.ReadRepository<File>();
            var fileWriteRepository = _unitOfWork.WriteRepository<File>();
            
            var file = await fileReadRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == request.UserId && x.Id == request.FileId);

            var userInfo = await _unitOfWork.ReadRepository<UserInfo>().FirstOrDefaultAsync(x => x.ApplicationUserId == request.UserId);

            userInfo.UsedSpace -= file.Size;

            _unitOfWork.WriteRepository<UserInfo>().Update(userInfo);
            
            fileWriteRepository.Remove(file);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<NoContent>.Fail("Veri silme sırasında hata meydana geldi", 500);
            }

            return Response<NoContent>.Success(200);


        }
    }
}
