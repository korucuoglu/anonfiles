using Microsoft.AspNetCore.Http;

namespace FileUpload.MVC.Models
{
    public class UploadFileDto
    {
        public IFormFile[] Files { get; set; }
    }
}
