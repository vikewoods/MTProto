using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTProto.Core.Types
{
  public interface ITLType
  {
    string TypeAlias { get; }
    int Length { get; }
    byte[] Serialize();
  }
}
