﻿using AutoMapper;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Commands.Categories.Update
{
    public class UpdateCategoryCommand : IRequest<Response<bool>>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id boş olamaz");
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title boş olamaz");
        }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Category>();

            if (! await repository.Any(x => x.UserId == request.UserId && x.Id == request.Id))
            {
                return Response<bool>.Fail(false, 200);
            }

            var category = _mapper.Map<Category>(request);
            await repository.Update(category);

            bool result = await _unitOfWork.Commit();

            if (!result)
            {
                return Response<bool>.Fail("Kayıt esnasında hata meydana geldi", 500);
            }

            return Response<bool>.Success(true, 200);
        }
    }
}
