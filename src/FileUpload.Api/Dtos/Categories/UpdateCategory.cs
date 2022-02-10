using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Api.Dtos.Categories
{
    public class UpdateCategory
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }

    public class UpdateCategoryValidator : AbstractValidator<UpdateCategory>
    {

        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage(" Kategori ismi boş olamaz");
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage(" Id boş olamaz");
        }
    }
}
