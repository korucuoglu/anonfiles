using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Shared.Models.Files
{
    public  class FileFilterModel
    {

        public int Page { get; set; } = 1;
        public int Number { get; set; } = 10;
        public int OrderBy { get; set; } = 0;
        public string Extension { get; set; }

    }
}
