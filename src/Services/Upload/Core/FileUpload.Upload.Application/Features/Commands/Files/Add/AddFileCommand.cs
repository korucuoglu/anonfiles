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
using FileUpload.Shared.Dtos.Files;
using AutoMapper;

namespace FileUpload.Upload.Application.Features.Commands.Files.Add
{
    public class AddFileCommand : IRequest<Response<AddFileDto>>
    {
        public File File { get; set; }
    }
    public class AddFileCommandValidator : AbstractValidator<AddFileCommand>
    {
        public AddFileCommandValidator()
        {
            RuleFor(x => x.File).NotNull().NotEmpty().WithMessage("Lütfen dosyayı giriniz");
        }
    }

    public class AddFileCommandHandler : IRequestHandler<AddFileCommand, Response<AddFileDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddFileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<AddFileDto>> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {

            var userInfo = await _unitOfWork.ReadRepository<UserInfo>().FirstOrDefaultAsync(x => x.ApplicationUserId == request.File.ApplicationUserId);

            userInfo.UsedSpace += request.File.Size;

            _unitOfWork.WriteRepository<UserInfo>().Update(userInfo);

            var data = await _unitOfWork.WriteRepository<File>().AddAsync(request.File);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<AddFileDto>.Fail("Dosyanın kaydedilmesi sırasında hata meydana geldi", 500);
            }

            var mapperData = _mapper.Map<AddFileDto>(data);

            return Response<AddFileDto>.Success(mapperData, 200);
        }
    }
}
