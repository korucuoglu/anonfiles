using AutoMapper;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Dtos.Categories;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Commands.Categories.Add
{
    public class AddCategoryCommand : IRequest<Response<GetCategoryDto>>
    {
        public string Title { get; set; }

        public string UserId { get; set; }
    }
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title boş olamaz");
        }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Response<GetCategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddCategoryCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
             _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<GetCategoryDto>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            var entity = await _unitOfWork.GetRepository<Category>().AddAsync(category);

            bool result = await _unitOfWork.Commit();

            if (!result)
            {
                return Response<GetCategoryDto>.Fail("Hata meydana geldi", 500);
            }

            GetCategoryDto dto = _mapper.Map<GetCategoryDto>(entity);

            return Response<GetCategoryDto>.Success(dto, 200);
        }
    }
}
