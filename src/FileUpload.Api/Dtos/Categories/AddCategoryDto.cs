using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Api.Dtos.Categories
{
    public class  AddCategoryDto
    {
        public string Title { get; set; }
    }

    public class AddCategoryDtoValidator : AbstractValidator<AddCategoryDto>
    {
        public AddCategoryDtoValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(" Kategori ismi boş olamaz");
        }
    }
}
