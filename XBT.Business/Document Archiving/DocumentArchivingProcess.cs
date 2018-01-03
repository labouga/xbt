using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBT.Data;
using XBT.Model;

namespace XBT.Business
{
  /// <summary>
  /// Contains methods responsible for the document indexing and archiving processes
  /// </summary>
  public class DocumentArchivingProcess
  {
    private XbtContext uow;

    /// <summary>
    /// Constructor
    /// </summary>
    public DocumentArchivingProcess()
    {
      uow = new XbtContext();
    }

    /// <summary>
    /// Retrieves all document groups
    /// </summary>
    /// <returns>IEnumerable list of document groups</returns>
    public IEnumerable<DocumentGroup> GetDocumentGroups()
    {
      var repo = new GenericRepository<DocumentGroup>(uow);

      //Retreive all document groups
      return repo.Get().ToList();
    }

    /// <summary>
    /// Retrieves document types in a selected document group
    /// </summary>
    /// <returns>IEnumerable list of document types</returns>
    public IEnumerable<DocumentType> GetDocumentTypesInDocumentGroup(int documentGroupId)
    {
      var repo = new GenericRepository<DocumentType>(uow);

      //Retreive all document groups
      return repo.Get(includeProperties: "DocumentGroup").Where(d => d.DocumentGroup.Id == documentGroupId).ToList();
    }

    /// <summary>
    /// Retrieves document tags in a selected document type
    /// </summary>
    /// <returns>IEnumerable list of document types</returns>
    public IEnumerable<DocumentTag> GetDocumentTagsInDocumentType(int documentTypeId)
    {
      var repo = new GenericRepository<DocumentType>(uow);

      //Retreive all document tags in a document type
      var docType = repo.Get(includeProperties: "DocumentTags").FirstOrDefault(d => d.Id == documentTypeId);

      return docType != null ? docType.DocumentTags : null;
    }

    ///<summary>
    /// Retreives a list files names from a directory which contains files for a sppecified documentype
    /// </summary>
    /// <param name="documentTypeId"></param>
    /// <returns>Queue of files</returns>
    public Queue<string> GetFileNamesInDirectory(int documentTypeId)
    {
      var fileQueue = new Queue<string>();

      //Retrieve document type 
      var repo = new GenericRepository<DocumentType>(uow);
      var selectedDocumentType = repo.GetById(documentTypeId);

      //If the document type was retreived successfully..
      if (selectedDocumentType != null)
      {
        //Retrieve directory where the files are stored
        string documentTypeFilesDirectory = Path.GetFullPath(selectedDocumentType.DocumentDirectory);

        string[] fileList;
        try
        {
          //Retrieve documents file names for the selected document type and add them to a queue for this document type 
           fileList = Directory.GetFiles(documentTypeFilesDirectory);
        }
        catch (Exception)
        {
          //TODO: Handle exceptions 
          return null;
        }
        
        //If there were files found in the directory
        if (fileList.GetLength(0) > 0)
        {
          //Add files to the queue
          foreach (string fileName in fileList)
            fileQueue.Enqueue(fileName);

          return fileQueue;
        }
      }
      return null;
    }

    public IEnumerable<Document> GetMyDocuments()
    {
      var repo = new GenericRepository<Document>(uow);
      return repo.Get(includeProperties: "DocumentType");
    }

    /// <summary>
    /// Saves a document to the database. The document is also moved from its current location to the specified archive forlder
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    public bool SaveDocument(Document document)
    {
      //Generate serial number
      document.SerialNo = GenerateDocumentSerialNumber();

      //Generate security hash for the document file to prevent swapping of stored files in the archive
      //TODO: 

      //Move file from current directory to archive 
      //TODO:

      //Save document metadata
      var repo = new GenericRepository<Document>(uow);
      repo.Insert(document);

      int saveStatus = 0;

      try
      {
        saveStatus = uow.SaveChanges();
      }
      catch (DbEntityValidationException ve)
      {
        //Catch validation errors
      }
      catch (Exception ex) //Other sql, connectivity exceptions. 
      {
        throw ex; //The rest will be handled by controll Handle error methods
      }
      //The rest of the exception 

      //if insert was not successful
      if (saveStatus < 1)
      {
        return false;
      }

      return true;
    }

    #region Helper Methods

    /// <summary>
    /// //Generates serial number takes the format yymm-currentdocumentcount+1
    /// </summary>
    /// <returns>serial number</returns>
    public string GenerateDocumentSerialNumber()
    {
      var documentSequence = uow.Documents.Count() + 1;
      var yymm = DateTime.Now.ToString("yy") + DateTime.Now.ToString("mm");

      return string.Format("{0}-{1}", yymm, documentSequence.ToString("D5"));
    }

    #endregion
    
  }

}

