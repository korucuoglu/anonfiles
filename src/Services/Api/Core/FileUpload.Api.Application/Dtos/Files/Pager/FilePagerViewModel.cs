using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Application.Dtos.Files.Pager
{
    public class FilePagerViewModel
    {
        public Pager Pages { get; set; }

        public GetFileDto File { get; set; }

    }
}
