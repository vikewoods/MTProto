using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTProto.Core.Types
{
  public abstract class TLBase : ITLType
  {
    public abstract string TypeAlias { get; }
    public abstract int Length { get; }
    public abstract byte[] Serialize();
    public abstract T GetValue<T>();
  }
}
