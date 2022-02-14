using FileUpload.Shared.Dtos.Categories;
using FluentValidation;

namespace FileUpload.Api.Validation.CategoryDto
{
    public class GetCategoryDtoValidator : AbstractValidator<GetCategoryDto>
    {
        public GetCategoryDtoValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(" Kategori ismi boş olamaz");
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage(" Id boş olamaz");
        }
    }
}
