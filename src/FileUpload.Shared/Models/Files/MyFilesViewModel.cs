﻿using FileUpload.Shared.Models.Files;
using System;
using System.Collections.Generic;

namespace FileUpload.Shared.Models.Files
{
    public class MyFilesViewModel
    {
        public Pager Pages { get; set; }
        public List<FileDto> Files { get; set; }
      
    }

}
