using FileUpload.Api.Application.Dtos.Categories;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Commands.Files.Add
{
    public class AddFileCommand : IRequest<Response<bool>>
    {
        public List<File> Files { get; set; }
        public string UserId { get; set; }
        public List<FileCategory> FileCategories { get; set; }
    }
    public class AddFileCommandValidator : AbstractValidator<AddFileCommand>
    {
        public AddFileCommandValidator()
        {
            RuleFor(x => x.Files).NotNull().NotEmpty().WithMessage("Lütfen dosyayı giriniz");
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
            await _unitOfWork.GetRepository<File>().AddRangeAsync(request.Files);

            if (request.FileCategories.Any())
            {
                await _unitOfWork.GetRepository<FileCategory>().AddRangeAsync(request.FileCategories);
            }

            var userRepository = _unitOfWork.GetRepository<User>();

            if (await userRepository.Any(x => x.Id == request.UserId))
            {
                var userInfo = await userRepository.FirstOrDefaultAsync(x => x.Id == request.UserId);

                userInfo.UsedSpace += request.Files.Sum(x => x.Size);

                await userRepository.Update(userInfo);
            }
            else
            {
                User user = new User()
                {
                    Id = request.UserId,
                    UsedSpace = request.Files.Sum(x => x.Size)
                };

                await userRepository.AddAsync(user);
            }

            return Response<bool>.Success(true, 200);
        }
    }
}
