using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBT.Model;

namespace XBT.Data
{
  /// <summary>
  /// UnitOfWork ensures that the multiple repositories defined in here share a single database context. 
  /// </summary>
  public class UnitOfWork : IUnitOfWork, IDisposable
  {

    #region Private Properties

    private XbtContext context = new XbtContext();
    private GenericRepository<Document> documentRepository;
    private GenericRepository<DocumentGroup> documentGroupRepository;

    #endregion

    #region Constructors

    public UnitOfWork()
    {
      
    }
    #endregion

    #region Public Properties

    public GenericRepository<Document> DocumentRepository
    {
      get
      {

        if (this.documentRepository == null)
        {
          this.documentRepository = new GenericRepository<Document>(context);
        }
        return documentRepository;
      }
    }

    public GenericRepository<DocumentGroup> DocumentGroupRepository
    {
      get
      {

        if (this.documentGroupRepository == null)
        {
          this.documentGroupRepository = new GenericRepository<DocumentGroup>(context);
        }
        return documentGroupRepository;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Saves changes to the database
    /// </summary>
    public void Save()
    {
      context.SaveChanges();
    }

    private bool disposed = false;


    /// <summary>
    /// Disposes the context 
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          context.Dispose();
        }
      }
      this.disposed = true;
    }

    /// <summary>
    /// Marks context for gabage collection 
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion

  }
}
