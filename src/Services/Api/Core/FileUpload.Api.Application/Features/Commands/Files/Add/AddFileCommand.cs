﻿using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Files.Add
{
    public class AddFileCommand : IRequest<Response<bool>>
    {
        public List<File> Files { get; set; }
        public Guid AplicationUserId { get; set; }
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
            var userInfoReadRepository = _unitOfWork.ReadRepository<UserInfo>();

            if (userInfoReadRepository.Any(x => x.ApplicationUserId == request.AplicationUserId))
            {
                (await userInfoReadRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == request.AplicationUserId)).UsedSpace += request.Files.Sum(x => x.Size);
            }
            else
            {
                var userInfoModel = new UserInfo()
                {
                    ApplicationUserId = request.AplicationUserId,
                    UsedSpace = request.Files.Sum(x => x.Size)
                };

                var userInfoWriteRepository = _unitOfWork.WriteRepository<UserInfo>();

                await userInfoWriteRepository.AddAsync(userInfoModel);
            }

            await _unitOfWork.WriteRepository<File>().AddRangeAsync(request.Files);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<bool>.Fail(result, 200);
            }

            return Response<bool>.Success(result, 200);
        }
    }
}
