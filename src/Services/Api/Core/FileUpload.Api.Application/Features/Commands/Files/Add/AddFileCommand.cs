using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Files.Add
{
    public class AddFileCommand : IRequest<Response<bool>>
    {
        public File File { get; set; }
        public List<GetCategoryDto> Categories { get; set; }
    }
    public class AddFileCommandValidator : AbstractValidator<AddFileCommand>
    {
        public AddFileCommandValidator()
        {
            RuleFor(x => x.File).NotNull().NotEmpty().WithMessage("Lütfen dosyayı giriniz");
            RuleFor(x => x.File.Size).GreaterThan(0).WithMessage("Lütfen dolu bir dosya giriniz");
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
            (await _unitOfWork.GetRepository<UserInfo>().FirstOrDefaultAsync(x => x.ApplicationUserId == request.File.ApplicationUserId)).UsedSpace += request.File.Size;
            await _unitOfWork.GetRepository<File>().AddAsync(request.File);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<bool>.Fail(result, 200);
            }

            return Response<bool>.Success(result, 200);
        }
    }
}
