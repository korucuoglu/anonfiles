using FileUpload.Api.Dtos.File;
using FluentValidation;

namespace FileUpload.Api.Validation.File
{
    public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
    {
        public UploadFileDtoValidator()
        {
            RuleFor(x => x.File).NotNull().NotEmpty().WithMessage("Lütfen dosyayı giriniz");
            RuleFor(x => x.File.Length).GreaterThan(0).WithMessage("Lütfen içerisinde veri olan bir dosya giriniz. Boş dosya yüklenemez");
        }

    }
}
