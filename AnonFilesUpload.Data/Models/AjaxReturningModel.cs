using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonFilesUpload.Data.Models
{
    public class AjaxReturningModel
    {
        public string fileId { get; set; }
        public string fileName { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }

}
