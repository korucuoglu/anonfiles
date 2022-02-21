using FileUpload.Application.Dtos.Files;
using FileUpload.Application.Interfaces.Repositories;
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
        private readonly IRepository<File> _fileRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<FileCategory> _filesCategoriesRepository;

        public DeleteFileCommandHandler(IRepository<Domain.Entities.File> fileRepository,
            IRepository<UserInfo> userInfoRepository,
            IRepository<FileCategory> filesCategoriesRepository)
        {
            _fileRepository = fileRepository;
            _userInfoRepository = userInfoRepository;
            _filesCategoriesRepository = filesCategoriesRepository;

        }

        public async Task<Response<MyFileViewModel>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == request.UserId && x.Id == request.FileId);

            (await _userInfoRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == request.UserId)).UsedSpace -= file.Size;
            
            var data =  await Helper.Filter.GetOneFileAfterRemovedFile(_fileRepository.Where(x => x.ApplicationUserId == request.UserId), request.FilterModel);

            _fileRepository.Remove(file);

            return data;

        }
    }
}
