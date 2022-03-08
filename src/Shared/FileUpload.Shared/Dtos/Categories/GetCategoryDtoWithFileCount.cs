using System;

namespace FileUpload.Shared.Dtos.Categories
{
    public class GetCategoryDtoWithFileCount
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
    }

    
}
