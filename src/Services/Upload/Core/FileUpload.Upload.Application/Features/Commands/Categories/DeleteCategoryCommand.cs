﻿using FileUpload.Shared.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Redis;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Categories
{
    public class DeleteCategoryCommand : IRequest<Response<NoContent>>
    {
        public string Id { get; set; }
    }

    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage($"Id boş olamaz");
        }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Response<NoContent>>
    {
        private readonly IHashService _hashService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IRedisService redisService, ISharedIdentityService sharedIdentityService, IHashService hashService)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
            _sharedIdentityService = sharedIdentityService;
            _hashService = hashService;
        }

        public async Task<Response<NoContent>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.CategoryWriteRepository().
                RemoveAsync(x => x.Id == _hashService.Decode(request.Id) && x.UserId == _sharedIdentityService.GetUserId);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<NoContent>.Fail("Veri silinemedi", 500);
            }

            await _redisService.RemoveAsync($"categories-{request.Id}");

            return Response<NoContent>.Success(204);
        }
    }
}
