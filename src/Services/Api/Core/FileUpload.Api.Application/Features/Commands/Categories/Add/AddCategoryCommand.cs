using AutoMapper;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Categories.Add
{
    public class AddCategoryCommand : IRequest<Response<GetCategoryDto>>
    {
        public string Title { get; set; }

        public Guid ApplicationUserId { get; set; }
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
            var entity = await _unitOfWork.WriteRepository<Category>().AddAsync(category);
            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<GetCategoryDto>.Fail("Kaydetme sırasında hata meydana geldi",  500);
            }

            GetCategoryDto dto = _mapper.Map<GetCategoryDto>(entity);   

            return Response<GetCategoryDto>.Success(dto, 200);
        }
    }
}
