using FileUpload.MVC.Models.Files;
using System;
using System.Collections.Generic;

namespace FileUpload.MVC.Models.Files
{
    public class MyFilesViewModel
    {

        public MyFilesViewModel()
        {
            Files = new List<FileDto>();
            Pages = new Pager();
        }

        public Pager Pages { get; set; }
        public List<FileDto> Files { get; set; }
      
    }

}
