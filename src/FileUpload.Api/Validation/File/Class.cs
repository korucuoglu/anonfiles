using FileUpload.Shared.Models;
using FluentValidation;

namespace FileUpload.Api.Validation.File
{
    public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
    {
        public UploadFileDtoValidator()
        {
            RuleFor(x => x.Files).NotNull().NotEmpty().WithMessage("Lütfen dosyayı giriniz");
            // RuleFor(x => x.Files.Length).GreaterThan(0).WithMessage("Lütfen içerisinde veri olan bir dosya giriniz. Boş dosya yüklenemez");
        }

    }
}
