using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XBT.Business;
using XBT.Model;

namespace XBT.Presentation.Areas.DigitalArchive.Controllers
{
  /// <summary>
  /// Contains methods (actions) responsible for creating, editing, deleting, previewing and listing of documents
  /// </summary>
  public class DocumentsController : Controller
  {
    #region Private Methods

    private DocumentArchivingProcess _documentArchivingProcess;
    private DocumentTypeBL _documentTypeBL;
    private CustomerBL _customerBL;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    public DocumentsController()
    {
      _documentArchivingProcess = new DocumentArchivingProcess();
      _documentTypeBL = new DocumentTypeBL();
      _customerBL = new CustomerBL();
    }

    #endregion

    #region Document Create Methods

    /// <summary>
    /// Displays the document creation form
    /// </summary>
    [HttpGet]
    public ActionResult Create()
    {
      ViewBag.Section = "Documents";
      return View();
    }

    /// <summary>
    /// Handles creation of the document process
    /// </summary>
    [HttpPost]
    public JsonResult Create(
        string documentType,
        string documentTags,
        string documentOwner,
        string acountNumber,
        string description)
    {
      string[] tagList = null;
      int docOwnerId;
      int docTypeId;
      var saveStatus = false;

      if (Int32.TryParse(documentOwner.Split(',')[0], out docOwnerId)
          && Int32.TryParse(documentType, out docTypeId))
      {
        var docOwner = _customerBL.GetCustomer(docOwnerId);
        var docType = _documentTypeBL.GetDocumentTypeById(docTypeId);

        //Initialize list to hold documents to be added to document
        var docTags = new List<DocumentTag>();

        //Retreive documents from string of documentTags 
        if (!String.IsNullOrEmpty(documentTags))
        {
          //Get array of documentTagIds from string of documentTags from request
          tagList = documentTags.Split(',');

          //Retreive documents tags belonging to the selected document tag
          var docTagList = _documentArchivingProcess.GetDocumentTagsInDocumentType(docTypeId);

          //Get docs to be added to document
          foreach (var tag in tagList.ToList())
          {
            var docTag = docTagList.FirstOrDefault(d => d.Id == Int32.Parse(tag));
            docTags.Add(docTag);
          }
        }

        var document = new Document()
        {
          DocumentType = docType,
          Description = description,
          DocumentOwner = docOwner,
          DocumentTags = docTags
        };

        //Save document 
        saveStatus = _documentArchivingProcess.SaveDocument(document);
      }

      return Json(saveStatus ? 1 : 0, JsonRequestBehavior.AllowGet);
    }

    #endregion

    #region Document Edit Methods

    /// <summary>
    /// Displays the form for editing a document
    /// </summary>
    /// <returns>The document edit view</returns>

    [HttpGet]
    public ActionResult Edit(int id = 0)
    {
      return View();
    }

    /// <summary>
    /// Handles the processes and logic of editing a document
    /// </summary>
    [HttpPost]
    public ActionResult Edit(Document document)
    {
      return View();
    }

    #endregion

    #region Delete Document Methods

    /// <summary>
    /// Handles the processes and logic of deleting a document
    /// </summary>
    [HttpPost]
    public JsonResult Delete(int documentId = 0)
    {
      return Json("", JsonRequestBehavior.AllowGet);
    }

    #endregion

    #region Document Retrieval Methods

    /// <summary>
    /// Retrieves the details of a document
    /// </summary>
    public ActionResult Document(int documentId = 0)
    {
      return View();
    }

    /// <summary>
    /// Retrieves all documents
    /// </summary>
    /// <returns>IEnumerable list of documents</returns>
    ///
    /// GET: /DigitalArchive/Documents/MyDocuments/
    /// 
    [HttpGet]
    public ActionResult MyDocuments()
    {
      ViewBag.Section = "Documents";
      return View();
    }

    [HttpGet]
    public JsonResult FetchMyDocuments()
    {
      var myDocuments = _documentArchivingProcess.GetMyDocuments();

      var docs = new List<object>();

      foreach (var doc in myDocuments)
      {
        docs.Add(new
        {
          SerialNo = doc.SerialNo,
          Description = doc.Description,
          DocumentType = doc.DocumentType.Name,
          DocumentTags = "Current Account, Savings Account",
          DateCreated = doc.DateCreated.ToString("D", new CultureInfo("nb-NO"))
        });
      }


      return Json(docs.ToList(), JsonRequestBehavior.AllowGet);
    }

    #endregion

    #region document navigation

    /// <summary>
    /// Retreives a queue of files names from the selected directory and adds it to the Cache object
    /// for sequential access i.e one by one
    /// </summary>
    /// <param name="documentTypeId"></param>
    /// <returns>Json result indicating wthether files where found or not</returns>
    public ActionResult GetFileList(int documentTypeId = 0)
    {
      string message = "";
      Object result = null;

      //retreive queue from Cache
      var cachedFileQueue = GetFileListQueue(documentTypeId);

      //if there are files in the queue
      if (cachedFileQueue.Count > 0)
      {
        message = "";
        result = new { Result = "true", message };
      }
      else
      {
        message = "No documents found in selected directory";
        result = new { Result = "false", message };
      }

      return Json(result, JsonRequestBehavior.AllowGet);
    }

    public FileResult FetchNextDocument(int documentTypeId)
    {
      //Retrieve queue
      var documentType = _documentTypeBL.GetDocumentTypeById(documentTypeId);

      var fileListQueue = GetFileListQueue(documentTypeId);


      if (fileListQueue.Count > 0)
      {
        //Fetch next file name in the queue
        var fileToRetrieveName = fileListQueue.Dequeue();

        //Retrieve directory where the files are stored
        var documentTypeFilesDirectory = Path.GetFullPath(documentType.DocumentDirectory);
        var absoluteFilePath = Path.GetFullPath(Path.Combine(documentType.DocumentDirectory, fileToRetrieveName));

        //Ensure that file name retrieved exists
        //TODO: Exception handling for IO operations
        if (System.IO.File.Exists(absoluteFilePath))
        {
          //Otherwise return file to client
          const string contentType = "application/pdf";
          return File(absoluteFilePath, contentType);
        }
      }

      //TODO: Explorer different status messages to send to client
      return null;
    }

    #endregion

    #region Helper methods

    /// <summary>
    /// Helper methods for retreiving a queue of files names from the selected directory and adds it to the Cache object
    /// for sequential access i.e one by one
    /// </summary>
    private Queue<string> GetFileListQueue(int documentTypeId)
    {
      //Create cache for fileName queue for selected document type 
      var documentType = _documentTypeBL.GetDocumentTypeById(documentTypeId);

      if (HttpRuntime.Cache.Get(documentType.Name) == null)
      {
        //Retreive queue //TODO: Potential return of no values
        Queue<string> fileQueue = _documentArchivingProcess.GetFileNamesInDirectory(documentTypeId);
        HttpRuntime.Cache.Insert(documentType.Name, fileQueue);
      }

      //retreive queue from Cache 
      return (Queue<string>)HttpRuntime.Cache.Get(documentType.Name);
    }

    #endregion

    //To be moved to customer controller
    #region Customer Documents

    /// <summary>
    /// Returns view for diplaying customer documents
    /// </summary>
    public ActionResult CustomerDocuments(int customerNumber = 0)
    {
      return View();
    }

    /// <summary>
    /// Retrieves all the documents for a customer
    /// </summary>
    public JsonResult GetAllCustomerDocuments(int customerNumber = 0)
    {
      // Get the customer documents

      // TODO: Implement the filter
      var customerDocuments = _customerBL.GetCustomerDocuments(customerNumber).Take(10);

      var docs = new List<object>();

      foreach (var doc in customerDocuments)
      {
        docs.Add(new
        {
          SerialNo = doc.SerialNo,
          Description = doc.Description,
          DocumentType = doc.DocumentType.Name,
          DocumentTags = "Current Account, Savings Account",
          DateCreated = doc.DateCreated.ToString("D", new CultureInfo("nb-NO"))
        });
      }

      return Json(docs.ToList(), JsonRequestBehavior.AllowGet);
    }

    #endregion
  }
}
