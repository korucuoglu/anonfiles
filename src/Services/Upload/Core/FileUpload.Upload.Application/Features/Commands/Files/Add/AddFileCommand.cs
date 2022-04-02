using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Files.Add
{
    public class AddFileCommand : IRequest<Response<bool>>
    {
        public File File { get; set; }
        public int AplicationUserId { get; set; }
    }
    public class AddFileCommandValidator : AbstractValidator<AddFileCommand>
    {
        public AddFileCommandValidator()
        {
            RuleFor(x => x.File).NotNull().NotEmpty().WithMessage("Lütfen dosyayı giriniz");
        }
    }

    public class AddFileCommandHandler : IRequestHandler<AddFileCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddFileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {

            var userInfo = await _unitOfWork.ReadRepository<UserInfo>().FirstOrDefaultAsync(x => x.ApplicationUserId == request.AplicationUserId);

            userInfo.UsedSpace += request.File.Size;

            _unitOfWork.WriteRepository<UserInfo>().Update(userInfo);

            await _unitOfWork.WriteRepository<File>().AddAsync(request.File);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<bool>.Fail(result, 200);
            }

            return Response<bool>.Success(result, 200);
        }
    }
}
