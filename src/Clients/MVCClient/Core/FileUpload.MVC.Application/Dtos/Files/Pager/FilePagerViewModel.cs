using FileUpload.MVC.Application.Dtos.Categories;
using System.Collections.Generic;

namespace FileUpload.MVC.Application.Dtos.Files.Pager
{
    public class FilePagerViewModel
    {
        public Pager Pages { get; set; }
        public GetFileDto File { get; set; }
        public List<GetCategoryDto> Categories { get; set; }
      
    }

}
