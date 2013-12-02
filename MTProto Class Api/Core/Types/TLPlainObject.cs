using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto_Class_Api.Core.Types
{
  class TLPlainObject : ITLType
  {
    public byte[] RawValue { get; set; }

    public TLPlainObject(byte[] o)
    {
      this.RawValue = o;
    }

    public string TypeAlias
    {
      get { return "Object"; }
    }

    public int Length
    {
      get { return RawValue.Length; }
    }

    public byte[] Serialize()
    {
      return RawValue;
    }
  }
}
