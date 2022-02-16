using ByteSizeLib;
using FileUpload.Shared.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileUpload.Shared.Helper
{
    public static class Helper
    {
        public static string GetUsedSpace(double size)
        {
            var UsedByte = ByteSize.FromBytes(size);

            if (UsedByte.GigaBytes > 1)
            {
                return $"{Math.Round(UsedByte.GigaBytes, 2)} GB";
            }

            if (UsedByte.MegaBytes > 1)
            {
                return $"{Math.Round(UsedByte.MegaBytes, 2)} MB";
            }

            if (UsedByte.KiloBytes > 1)
            {
                return $"{Math.Round(UsedByte.KiloBytes, 2)} KB";
            }

            return $"{Math.Round(UsedByte.KiloBytes, 2)} Byte";


        }

        public static string GetRemainingSpace(double size)
        {
            var maxSizeForByte = ByteSize.FromGigaBytes(100).Bytes;

            var remainingSpace = Math.Floor((size * 100) / maxSizeForByte);

            return $"% {remainingSpace}";
        }

        public static async Task<MultipartContent> GetMultipartContentAsync(UploadFileDto dto)
        {
            var multipartContent = new MultipartFormDataContent();

            using var ms = new MemoryStream();

            foreach (var file in dto.Files)
            {
                await file.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "files", file.FileName);
            }


            byte[] SerializeObject(object value) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));

            //var stringContent = new StringContent(JsonConvert.SerializeObject(dto.Categories), Encoding.UTF8, "application/json");

            //multipartContent.Add(stringContent, "categories");

            var byteArrayContent = new ByteArrayContent(SerializeObject(dto.Categories));

            multipartContent.Add(byteArrayContent, "Categories");


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
