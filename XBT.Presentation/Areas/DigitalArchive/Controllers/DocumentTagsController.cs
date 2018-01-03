using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XBT.Business;

namespace XBT.Presentation.Areas.DigitalArchive.Controllers
{
    /// <summary>
    /// Contains methods responsible for handling document tags
    /// </summary>
    public class DocumentTagsController : Controller
    {
        private DocumentArchivingProcess _documentArchivingProcess;
        public DocumentTagsController()
        {
            _documentArchivingProcess = new DocumentArchivingProcess();
        }

        /// POST: /DigitalArchive/DocumentTags/FilterByDocumentType
        /// <para>documentTypeId</para>
        /// <summary>
        /// Gets all document tags in a document type
        /// </summary>
        /// <returns>Returns a json object for all the document tags in a specified document type</returns>
        [HttpGet]
        public JsonResult FilterByDocumentType(int documentTypeId = 0)
        {
            // Get all the document tags in the document type
            var documentTags = _documentArchivingProcess.GetDocumentTagsInDocumentType(documentTypeId);

            var documentTagsList = new Dictionary<int, string>();
            if (documentTags != null)
            {
                if (documentTags.Any())
                {
                    foreach (var docTag in documentTags)
                    {
                        documentTagsList.Add(docTag.Id, docTag.Name);
                    }

                }
            }

            return Json(documentTagsList.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
