using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Shared.Models.Files
{
    public  class FileFilterModel
    {
        public int OrderBy { get; set; }

        private int page;
        public int Page
        {
            get => page;
            set => page = page == 0 ? 1 : value;
        }

        private int number;
        public int Number
        {
            get => number;
            set => number = number == 0 ? 10 : value;
        }
    }
}
