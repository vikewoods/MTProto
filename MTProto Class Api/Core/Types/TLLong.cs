using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTProto_Class_Api.Core.Types
{
  class TLLong : ITLType
  {
    public Int64 RawValue { get; set; }

    public TLLong(Int64 rawValue)
    {
      this.RawValue = rawValue;
    }

    public TLLong(byte[] bytes)
    {
      this.RawValue = BitConverter.ToInt64(bytes, 0);
    }

    public int Length
    {
      get { return 8; }
    }

    public string TypeAlias
    {
      get { return "long"; }
    }

    public byte[] Serialize()
    {
      return BitConverter.GetBytes(this.RawValue);
    }

    /// <summary>
    /// Переопределение оператора (long) 
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static explicit operator long(TLLong a)
    {
      return a.RawValue;
    }


    public void Set(object value)
    {
      if (value is long)
      {
        this.RawValue = (long)value;
      }
      else
      {
        throw new ArgumentException("Wrong boxed type");
      }
    }

    public override string ToString()
    {
      return RawValue.ToString("X");
    }
  }
}
