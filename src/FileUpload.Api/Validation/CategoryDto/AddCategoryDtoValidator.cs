using FileUpload.Shared.Dtos.Categories;
using FluentValidation;

namespace FileUpload.Api.Validation.CategoryDto
{
    public class AddCategoryDtoValidator : AbstractValidator<AddCategoryDto>
    {
        public AddCategoryDtoValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("Title değeri boş olamaz");
        }
    }
}
