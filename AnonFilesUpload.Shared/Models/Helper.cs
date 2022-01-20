using ByteSizeLib;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnonFilesUpload.Shared.Models
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

        public static async Task<MultipartContent> GetMultipartContentAsync(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                var multipartContent = new MultipartFormDataContent();
                await file.CopyToAsync(ms);
                multipartContent.Add(new ByteArrayContent(ms.ToArray()), "file", file.FileName);

                return multipartContent;
            };
        }

    }
}
