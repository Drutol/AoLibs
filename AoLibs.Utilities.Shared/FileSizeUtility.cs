using System;

namespace AoLibs.Utilities.Shared
{
    public static class FileSizeUtility
    {
        private static readonly string[] SizeSuffixes = {"B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};

        /// <summary>
        /// Converts byte count to nice formatted string with using one of following suffixes:
        /// "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
        /// </summary>
        /// <param name="value">Number of bytes.</param>
        public static string GetHumanReadableBytesLength(long value)
        {
            if (value < 0)
            {
                return "-" + GetHumanReadableBytesLength(-value);
            }

            if (value == 0)
            {
                return "0.0 bytes";
            }

            var mag = (int) Math.Log(value, 1024);
            var adjustedSize = (decimal) value / (1L << (mag * 10));

            return $"{adjustedSize:n1} {SizeSuffixes[mag]}";
        }
    }
}
