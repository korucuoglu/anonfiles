using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Shared.Dtos.User
{
    public class ResourceOwnerPasswordResponse
    {
        public ResourceOwnerPasswordResponse()
        {
            errors = new List<string>();
        }

        public string error { get; set; }
        public string error_description { get; set; }
        public List<string> errors { get; set; }
    }
}
