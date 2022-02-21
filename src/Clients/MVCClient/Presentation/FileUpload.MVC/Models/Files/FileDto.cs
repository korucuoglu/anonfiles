using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.MVC.Models.Files
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Size { get; set; }
    }
}
