using FileUpload.Shared.Dtos.Files;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.MVC.Infrastructure.Helper
{
    public static class Helper
    {
        public static async Task<MultipartContent> GetMultipartContentAsync(FilesCategoriesDto dto)
        {
            var multipartContent = new MultipartFormDataContent();

            using var ms = new MemoryStream();

            foreach (var file in dto.Files)
            {
                await file.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "files", file.FileName);
            }

            byte[] SerializeObject(object value) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));

            var byteArrayContent = new ByteArrayContent(SerializeObject(dto.CategoriesId));

            multipartContent.Add(byteArrayContent, "categories");


            return multipartContent;
        }
    }
}
