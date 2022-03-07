using System;

namespace FileUpload.Application.Dtos.Categories

{
    public class GetCategoryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
    }

    
}
