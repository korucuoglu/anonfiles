using FileUpload.Application.Dtos.Categories;
using System;
using System.Collections.Generic;

namespace FileUpload.Application.Dtos.Files
{
    public class GetFileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Size { get; set; }

    }
}
