using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XBT.Business;

namespace XBT.Presentation.Areas.DigitalArchive.Controllers
{

    /// <summary>
    /// Contains methods responsible for handling document groups
    /// </summary>
    public class DocumentGroupsController : Controller
    {
        private DocumentArchivingProcess _documentArchivingProcess;
        public DocumentGroupsController()
        {
            _documentArchivingProcess = new DocumentArchivingProcess();
        }

        /// POST: /DigitalArchive/DocumentGroups/GetAll
        /// <summary>
        /// Gets all document groups
        /// </summary>
        /// <returns>Returns a json object for all the document groups</returns>
        [HttpGet]
        public JsonResult FetchAll()
        {
            // Get all the document groups
            var documentGroups = _documentArchivingProcess.GetDocumentGroups();

            var documentGroupsList = new Dictionary<int, string>();
            if (documentGroups.Any())
            {
                foreach (var documentGroup in documentGroups)
                {
                    documentGroupsList.Add(documentGroup.Id, documentGroup.Name);
                }
            }

            return Json(documentGroupsList.ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
