using AutoMapper;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Categories.Add
{
    public class AddCategoryCommand : IRequest<Response<bool>>
    {
        public string Title { get; set; }
    }
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title boş olamaz");
        }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Response<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepository;

        public AddCategoryCommandHandler(IMapper mapper, IRepository<Category> categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<bool>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            await _categoryRepository.AddAsync(category);

            return Response<bool>.Success(true, 200);
        }
    }
}
