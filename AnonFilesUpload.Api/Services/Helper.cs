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

    }
}
