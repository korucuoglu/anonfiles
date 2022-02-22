using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Files.Delete
{
    public class DeleteFileCommand : IRequest<Response<MyFileViewModel>>
    {
        public Guid FileId { get; set; }
        public Guid UserId { get; set; }
        public FileFilterModel FilterModel { get; set; }
    }
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(x => x.FileId).NotNull().NotEmpty().WithMessage("FileId boş bırakılamaz");
            RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("UserId boş bırakılamaz");
        }
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Response<MyFileViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<MyFileViewModel>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var fileRepository = _unitOfWork.GetRepository<File>();
            
            var file = await fileRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == request.UserId && x.Id == request.FileId);

            (await _unitOfWork.GetRepository<UserInfo>().FirstOrDefaultAsync(x => x.ApplicationUserId == request.UserId)).UsedSpace -= file.Size;
            
            var data =  await Helper.Filter.GetOneFileAfterRemovedFile(fileRepository.Where(x => x.ApplicationUserId == request.UserId), request.FilterModel);

            fileRepository.Remove(file);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<MyFileViewModel>.Fail("Veri silme sırasında hata meydana geldi", 500);
            }

            return data;

        }
    }
}
