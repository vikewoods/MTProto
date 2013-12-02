using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto_Class_Api.Core.TransportLayer
{
    class FileWorker
    {


        /// <summary>
        /// Attempts to read an entire chunk into the given array; returns the size of chunk actually read.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="chunk"></param>
        /// <returns></returns>
        private static async Task<int> ReadChunkAsync(Stream stream, byte[] chunk)
        {
            int index = 0;
            while (index < chunk.Length)
            {
                int bytesRead = await stream.ReadAsync(chunk, index, chunk.Length - index).ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    break;
                }
                index += bytesRead;
            }
            return index;
        }

        /// <summary>
        /// Разделение файла на кусочки
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<List<FilePart>> SplitFile(string path)
        {
            // Create new FileInfo object and get the Length.
            FileInfo f = new FileInfo(path);
            long chunksNum = 1;
            long s1 = f.Length;

            int chunkLength = 512000; // 1КБ
            // 512000 512 КБ

            if (s1 > chunkLength)
            {
                chunksNum = s1 / chunkLength; // число кусков
                s1 = chunkLength;
            }

            byte[] chunk = new byte[chunkLength];
            byte[] buf = new byte[8];
            List<FilePart> lfp = new List<FilePart>();

            using (FileStream s = new FileStream(path, FileMode.Open))
            {
                for (int i = 0; i < chunksNum; i++)
                {
                    int read = await ReadChunkAsync(s, chunk).ConfigureAwait(false);
                    if (read > 0)
                    {
                        if (read != chunk.Length)
                        {
                            Array.Resize(ref chunk, read);
                        }

                        r.NextBytes(buf);
                        lfp.Add(new FilePart(BitConverter.ToInt64(buf, 0), i, chunk));
                    }
                }
            }
            return lfp;
        }

        private static Random r = new Random();
    }
}
