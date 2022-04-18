using System.Collections.Generic;

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
