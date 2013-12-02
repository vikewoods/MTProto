using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTProto_Class_Api.Core.Types
{
  public class TLInt : ITLType
  {
    public int RawValue { get; set; }
    public TLInt(int rawValue)
    {
      this.RawValue = rawValue;
    }
    public int Length
    {
      get { return 4; }
    }
    public string TypeAlias
    {
      get { return "int"; }
    }
    public byte[] Serialize()
    {
      return BitConverter.GetBytes(this.RawValue);
    }
  }
}
