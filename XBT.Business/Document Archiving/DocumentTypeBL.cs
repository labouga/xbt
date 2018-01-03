using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBT.Data;
using XBT.Model;

namespace XBT.Business
{
  /// <summary>
  /// Contains methods responsible for handling documentType operations
  /// </summary>
  public class DocumentTypeBL
  {
    private XbtContext uow;

    /// <summary>
    /// Constructor
    /// </summary>
    public DocumentTypeBL()
    {
      uow = new XbtContext();
    }

    /// <summary>
    /// Retreives a documentType by its Id
    /// </summary>
    /// <param name="documentTypeId"></param>
    /// <returns></returns>
    public DocumentType GetDocumentTypeById(int documentTypeId)
    {
      var repo = new GenericRepository<DocumentType>(uow);
      return repo.Get(includeProperties: "DocumentGroup").FirstOrDefault(d => d.Id == documentTypeId);
    }
  }
}
