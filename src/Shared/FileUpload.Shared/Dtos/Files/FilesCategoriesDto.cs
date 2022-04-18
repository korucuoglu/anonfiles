using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FileUpload.Shared.Dtos.Files
{
    public class FilesCategoriesDto
    {
        public FilesCategoriesDto()
        {
            CategoriesId = new List<int>();
        }

        public IFormFile[] Files { get; set; }

        public List<int> CategoriesId { get; set; }

    }
}
