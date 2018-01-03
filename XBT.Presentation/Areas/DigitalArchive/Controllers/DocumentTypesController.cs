using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XBT.Business;

namespace XBT.Presentation.Areas.DigitalArchive.Controllers
{
    /// <summary>
    /// Contains methods responsible for handling document types
    /// </summary>
    public class DocumentTypesController : Controller
    {
        private DocumentArchivingProcess _documentArchivingProcess;
        public DocumentTypesController()
        {
            _documentArchivingProcess = new DocumentArchivingProcess();
        }

        /// POST: /DigitalArchive/DocumentTypes/FilterByDocumentGroup
        /// <summary>
        /// Gets all document groups
        /// </summary>
        /// <returns>Returns a json object for all the document groups</returns>
        [HttpGet]
        public JsonResult FilterByDocumentGroup(int documentGroupId = 0)
        {
            // Get all the document types in the document group
            var documentTypes = _documentArchivingProcess.GetDocumentTypesInDocumentGroup(documentGroupId);

            var documentTypesList = new Dictionary<int, string>();

            foreach (var type in documentTypes)
            {
                documentTypesList.Add(type.Id, type.Name);
            }

            return Json(documentTypesList.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
