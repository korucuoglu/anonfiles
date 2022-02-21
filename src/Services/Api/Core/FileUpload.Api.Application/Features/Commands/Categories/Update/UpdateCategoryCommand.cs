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

namespace FileUpload.Application.Features.Commands.Categories.Update
{
    public class UpdateCategoryCommand : IRequest<Response<bool>>
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage($"Id boş olamaz");
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage($"Title boş olamaz");
        }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response<bool>>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IRepository<Category> categoryRepository, ISharedIdentityService sharedIdentityService, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _sharedIdentityService = sharedIdentityService;
            _mapper = mapper;
        }

        public async Task<Response<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!_categoryRepository.Any(x => x.ApplicationUserId == _sharedIdentityService.GetUserId && x.Id == new Guid(request.Id)))
            {
                return Response<bool>.Fail(false, 200);
            }

            var category = _mapper.Map<Category>(request);
            _categoryRepository.Update(category);
            return await Task.FromResult(Response<bool>.Success(true, 200));
        }
    }
}
