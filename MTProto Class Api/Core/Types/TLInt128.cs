using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Math;

namespace MTProto_Class_Api.Core.Types
{
  class TLInt128 : ITLType
  {
    public BigInteger RawValue { get; set; }
    public TLInt128(BigInteger bigInt)
    {
      this.RawValue = bigInt;
    }
    public TLInt128(byte[] bytes)
    {
      this.RawValue = new BigInteger(bytes.Reverse().ToArray());
    }
    public int Length
    {
      get { return 16; }
    }
    public string TypeAlias
    {
      get { return "int128"; }
    }
    public override string ToString()
    {
      return this.RawValue.ToString(16);
    }
    public byte[] Serialize()
    {
      return this.RawValue.GetBytes();
    }
    public void Set(object value)
    {
      this.RawValue = (BigInteger)value;
    }
  }
}
