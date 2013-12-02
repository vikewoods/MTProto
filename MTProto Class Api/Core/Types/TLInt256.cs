using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Math;

namespace MTProto_Class_Api.Core.Types
{
  class TLInt256 : ITLType
  {
    public BigInteger RawValue { get; set; }

    public TLInt256(BigInteger bigInt)
    {
      this.RawValue = bigInt;
    }

    public int Length
    {
      get { return 32; }
    }

    public string TypeAlias
    {
      get { return "int256"; }
    }

    public byte[] Serialize()
    {
      return this.RawValue.GetBytes();
    }


    public void Set(object value)
    {
      if (value is BigInteger)
      {
        this.RawValue = (BigInteger)value;
      }
      else
      {
        throw new ArgumentException("Wrong boxed type");
      }
    }
  }
}
