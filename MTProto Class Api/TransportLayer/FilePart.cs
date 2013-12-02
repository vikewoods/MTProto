using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Core.TransportLayer
{
    /// <summary>
    /// 
    /// </summary>
    /// file_id:long file_part:int bytes:bytes
    public class FilePart
    {
        public long FileId { get; set; }
        public int FilePartNum { get; set; }
        public byte[] Bytes { get; set; }

        public FilePart(long fileId, int num, byte[] data)
        {
            this.FileId = fileId;
            this.FilePartNum = num;
            this.Bytes = data;
        }
    }
}
