using FileUpload.Api.Application.Dtos.Categories;
using System.Collections.Generic;

namespace FileUpload.Api.Application.Dtos.Files.Pager
{
    public class FilesPagerViewModel
    {
        public FilesPagerViewModel()
        {
            Files = new List<GetFileDto>();
            Pages = new Pager();
            Categories = new List<GetCategoryDto>();
        }

        public Pager Pages { get; set; }
        public List<GetFileDto> Files { get; set; }
        public List<GetCategoryDto> Categories { get; set; }

    }
}
