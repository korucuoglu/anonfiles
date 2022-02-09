using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Data.Entity
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
