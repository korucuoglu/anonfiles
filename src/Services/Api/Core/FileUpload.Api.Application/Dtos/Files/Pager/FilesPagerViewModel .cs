using System.Collections.Generic;

namespace FileUpload.Application.Dtos.Files.Pager
{
    public class FilesPagerViewModel
    {
        public FilesPagerViewModel()
        {
            Files = new List<GetFileDto>();
            Pages = new Pager();
        }

        public Pager Pages { get; set; }
        public List<GetFileDto> Files { get; set; }

    }
}
