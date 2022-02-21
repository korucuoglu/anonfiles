using AutoMapper;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Categories.Delete
{
    public class DeleteCategoryCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
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
        private readonly IRepository<Category> _categoryRepository;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DeleteCategoryCommandHandler(IRepository<Category> categoryRepository, ISharedIdentityService sharedIdentityService)
        {
            _categoryRepository = categoryRepository;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(x => x.ApplicationUserId == _sharedIdentityService.GetUserId && x.Id == request.Id);
            
            _categoryRepository.Remove(category);

            return Response<bool>.Success(true, 200);
        }
    }
}
