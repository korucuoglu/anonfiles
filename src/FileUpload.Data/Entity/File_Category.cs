using FileUpload.Data.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Data.Entity
{
    public class File_Category
    {
        public int Id { get; set; }
        public Guid FileId { get; set; }
        public File File { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
