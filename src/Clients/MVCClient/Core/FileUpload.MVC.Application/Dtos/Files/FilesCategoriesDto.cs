using FileUpload.MVC.Application.Dtos.Categories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FileUpload.MVC.Application.Dtos.Files
{
    public class FilesCategoriesDto
    {
        public IFormFile[] Files { get; set; }

        public List<GetCategoryDto> Categories { get; set; }

    }
}
