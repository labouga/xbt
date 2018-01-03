using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBT.Data
{
  public interface IUnitOfWork
  {
    void Save();
  }
}
