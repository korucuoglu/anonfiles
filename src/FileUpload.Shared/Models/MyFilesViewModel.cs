using System;
using System.Collections.Generic;

namespace FileUpload.Shared.Models
{
    public class MyFilesViewModel
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedDate { get; set; }
        public long Size { get; set; }
    }

}
