using ByteSizeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonFilesUpload.Api.Services
{
    public static class Helper
    {
        public static string GetUsedSpace(double size)
        {
            var maxSize = ByteSize.FromBytes(size);

            if (maxSize.GigaBytes > 1)
            {
                return $"{maxSize.GigaBytes} GB";
            }

            if (maxSize.MegaBytes > 1)
            {
                return $"{maxSize.MegaBytes} MB";
            }

            if (maxSize.KiloBytes > 1)
            {
                return $"{maxSize.KibiBytes} KB";
            }

            return $"{maxSize.Bytes} Byte";

            
        }

        public static string GetRemainingSpace(double size)
        {
            var maxSizeForByte = ByteSize.FromGigaBytes(100).Bytes;

            var remainingSpace = (size * 100) / maxSizeForByte;

            return $"% {remainingSpace}";
        }

    }
}
