using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
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
        private readonly IRepository<File> _fileRepository;
        private readonly IRepository<UserInfo> _userInfoRepository;
        private readonly IRepository<FileCategory> _filesCategoriesRepository;

        public AddFileCommandHandler(IRepository<Domain.Entities.File> fileRepository,
            IRepository<UserInfo> userInfoRepository,
            IRepository<FileCategory> filesCategoriesRepository)
        {
            _fileRepository = fileRepository;
            _userInfoRepository = userInfoRepository;
            _filesCategoriesRepository = filesCategoriesRepository;
           
        }

        public async Task<Response<bool>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            (await _userInfoRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == request.File.ApplicationUserId)).UsedSpace += request.File.Size;
            await _fileRepository.AddAsync(request.File);

            return Response<bool>.Success(true, 200);
        }
    }
}
