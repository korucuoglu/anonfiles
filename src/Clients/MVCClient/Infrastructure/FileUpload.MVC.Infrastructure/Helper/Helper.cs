using FileUpload.MVC.Application.Dtos.Files;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

            var byteArrayContent = new ByteArrayContent(SerializeObject(dto.Categories));

            multipartContent.Add(byteArrayContent, "categories");


            return multipartContent;
        }
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }
    }
}
