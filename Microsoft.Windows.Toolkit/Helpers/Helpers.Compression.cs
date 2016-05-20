﻿using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Microsoft.Windows.Toolkit
{
    /// <summary>
    /// This class provides static helper methods.
    /// </summary>
    public static partial class Helpers
    {
        /// <summary>
        /// This method can be used to decompress a base64 string previously created by Core.Compress.
        /// </summary>
        /// <param name="data">String containing data to decompress.</param>
        /// <returns>Decompressed string</returns>
        public static string Decompress(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException();
            }

            using (var input = new MemoryStream(Convert.FromBase64String(data)))
            {
                using (var output = new MemoryStream())
                {
                    using (var gs = new GZipStream(input, CompressionMode.Decompress, true))
                    {
                        gs.CopyTo(output);
                    }
                    return Encoding.UTF8.GetString(output.ToArray(), 0, (int)output.Length);
                }
            }
        }

        /// <summary>
        /// This method returns a string containing a based64 compressed version of the given parameter.
        /// Compression will be done using GZip.
        /// Due to base64 conversion the original string has to be long enough to compensate the conversion overhead.
        /// </summary>
        /// <param name="data">String containing the data to compress.</param>
        /// <returns>Compressed string</returns>
        public static string Compress(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw  new ArgumentNullException();
            }

            var bytes = Encoding.UTF8.GetBytes(data);
            using (var output = new MemoryStream())
            {
                using (var gs = new GZipStream(output, CompressionLevel.Optimal, true))
                {
                    gs.Write(bytes, 0, bytes.Length);
                }

                output.Position = 0;

                return Convert.ToBase64String(output.ToArray());
            }
        }
    }
}