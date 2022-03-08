using FileUpload.Shared.Dtos.Categories;
using System.Collections.Generic;

namespace FileUpload.Shared.Dtos.Files.Pager
{
    public class FilePagerViewModel
    {
        public Pager Pages { get; set; }
        public GetFileDto File { get; set; }
        public List<GetCategoryDtoWithFileCount> Categories { get; set; }
      
    }

}
