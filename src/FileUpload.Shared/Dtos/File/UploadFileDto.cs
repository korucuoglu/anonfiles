using FileUpload.Shared.Dtos.Categories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FileUpload.Shared.Models
{
    public class UploadFileDto
    {
        public IFormFile[] Files { get; set; }

        public List<GetCategoryDto> Categories { get; set; }

    }
}
