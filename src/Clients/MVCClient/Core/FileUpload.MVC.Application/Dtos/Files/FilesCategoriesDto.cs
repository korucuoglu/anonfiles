using FileUpload.MVC.Application.Dtos.Categories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace FileUpload.MVC.Application.Dtos.Files
{
    public class FilesCategoriesDto
    {
        public FilesCategoriesDto()
        {
            CategoriesId = new List<Guid>();
        }

        public IFormFile[] Files { get; set; }

        public List<Guid> CategoriesId { get; set; }

    }
}
