using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MTProto.Core
{
    public static class BinaryExtensionMethods
    {
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }

        public static Task<int> ReadAsync(
this NetworkStream stream, byte[] buffer, int offset, int count)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            return Task<int>.Factory.FromAsync(stream.BeginRead, stream.EndRead,
                buffer, offset, count, null);
        }

        public static Task<int> WriteAsync(
this NetworkStream stream, byte[] buffer, int offset, int count)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            return Task<int>.Factory.FromAsync(stream.BeginRead, stream.EndRead,
                buffer, offset, count, null);
        }
    }
}
