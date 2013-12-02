using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTProto_Class_Api;
using Mono.Math;

namespace MTProto_Console
{
  class Program
  {
    
    static void Main(string[] args)
    {
      Datacenter datacenterController = new Datacenter();

      var i = new BigInteger(8798782624624);
      Console.WriteLine(i);
      Console.ReadLine();
    }
  }
}
