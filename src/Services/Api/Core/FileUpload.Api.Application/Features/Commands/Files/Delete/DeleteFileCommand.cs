using AutoMapper;
using FileUpload.Api.Application.Dtos.Files;
using FileUpload.Api.Application.Dtos.Files.Pager;
using FileUpload.Api.Application.Interfaces.Repositories;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Commands.Files.Delete
{
    public class DeleteFileCommand : IRequest<Response<FilePagerViewModel>>
    {
        public string FileId { get; set; }
        public string UserId { get; set; }
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
            var fileRepository = _unitOfWork.GetRepository<File>();

            var fileSize = fileRepository.Where(x => x.UserId == request.UserId && x.Id == request.FileId).Select(x => x.Size).FirstOrDefault();

            var userInfo = await _unitOfWork.GetRepository<User>().FirstOrDefaultAsync(x => x.Id == request.UserId);

            userInfo.UsedSpace -= fileSize;

            var data =  await Helper.Filter.GetDataInNextPageAfterRemovedFile(fileRepository.Where(x => x.UserId == request.UserId), request.FilterModel, _mapper);

            await fileRepository.Remove(x => x.UserId == request.UserId && x.Id == request.FileId);

            await _unitOfWork.GetRepository<User>().Update(userInfo);
            await _unitOfWork.GetRepository<FileCategory>().RemoveRange(x=> x.FileId == request.FileId);

            bool result = await _unitOfWork.Commit();

            if (!result)
            {
                return Response<FilePagerViewModel>.Fail("Kayıt esnasında hata meydana geldi", 500);
            }

            return data;

        }
    }
}
