using Microsoft.AspNetCore.Http;

namespace FileUpload.Api.Dtos.File
{
    public class UploadFileDto
    {
        public IFormFile File { get; set; }
    }

  
}
