﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Data.Entity
{
    public class Category: BaseEntity
    {
         
        public string Title { get; set; }
        public virtual ICollection<File> Files { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }



    }
}
