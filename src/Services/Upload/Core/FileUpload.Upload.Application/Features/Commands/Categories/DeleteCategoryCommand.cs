﻿using FileUpload.Upload.Application.Interfaces.Redis;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Categories
{
    public class DeleteCategoryCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage($"Id boş olamaz");
        }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }

        public async Task<Response<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.WriteRepository<Category>().RemoveAsync(x => x.ApplicationUserId == request.UserId && x.Id == request.Id);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<bool>.Fail(result, 200);
            }

            await _redisService.RemoveAsync($"categories-{request.Id}");

            return Response<bool>.Success(result, 200);
        }
    }
}
