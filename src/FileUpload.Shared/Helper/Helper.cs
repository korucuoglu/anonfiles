using ByteSizeLib;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        public static async Task<MultipartContent> GetMultipartContentAsync(IFormFile[] files)
        {
            var multipartContent = new MultipartFormDataContent();

            foreach (var file in files)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "files", file.FileName);
            }
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
