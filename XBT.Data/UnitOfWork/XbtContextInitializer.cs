using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace XBT.Data
{
  public class XbtContextInitializer : DropCreateDatabaseIfModelChanges<XbtContext>
  {
    public XbtContextInitializer()
    {

    }
  }
}
