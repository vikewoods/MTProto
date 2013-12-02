using MTProto.Core.ApiLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTProto.Core.TransportLayer
{
    /// <summary>
    /// Класс описывающий датацентр
    /// </summary>
    /// dcOption#2ec2a43c id:int hostname:string ip_address:string port:int = DcOption;
    [Serializable]
    public class DcOptions
    {
        public DcOptions()
        {

        }

        public DcOptions(TLCombinatorInstance tlci)
        {

        }

        public int Id { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }        
    }
}
