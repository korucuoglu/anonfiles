using FileUpload.Shared.Dtos.Categories;
using FluentValidation;

namespace FileUpload.Api.Validation.CategoryDto
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategory>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(" Kategori ismi boş olamaz");
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage(" Id boş olamaz");
        }
    }
}
