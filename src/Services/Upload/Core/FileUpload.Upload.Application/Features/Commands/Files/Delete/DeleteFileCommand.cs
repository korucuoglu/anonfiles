using AutoMapper;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Dtos.Files;
using FileUpload.Shared.Dtos.Files.Pager;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Files.Delete
{
    public class DeleteFileCommand : IRequest<Response<FilePagerViewModel>>
    {
        public int FileId { get; set; }
        public int UserId { get; set; }
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

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, Response<FilePagerViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteFileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<FilePagerViewModel>> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
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
                return Response<FilePagerViewModel>.Fail("Veri silme sırasında hata meydana geldi", 500);
            }

            return await Helper.Filter.GetDataInNextPageAfterRemovedFile(fileReadRepository.Where(x => x.ApplicationUserId == request.UserId), request.FilterModel, _mapper);

        }
    }
}
