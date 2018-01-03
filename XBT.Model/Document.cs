using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBT.Model
{
    /// <summary>
    /// Represents a document 
    /// </summary>
    public class Document
    {
        #region Constructors

        public Document()
        {
            //Generate unique identifier for the document 
            DocumentUniqueIdentifier = Guid.NewGuid();
            DateCreated = DateTime.Now.ToLocalTime();
        }

        #endregion

        #region Public Properties

        [Key]
        public int Id { get; set; }
        public Guid DocumentUniqueIdentifier { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string SerialNo { get; set; }
        public string FileName { get; set; }
        public DateTime DateCreated { get; set; }

        #endregion

        #region Associations

        //Properties for self referencing 
        public int? ParentDocumentId { get; set; }
        public virtual Document ParentDocument { get; set; }
        public virtual List<Document> ChildDocuments { get; set; }

        // [Required]
        public DocumentOwner DocumentOwner { get; set; }

        [Required]
        public DocumentType DocumentType { get; set; }

        public List<DocumentTag> DocumentTags { get; set; }

        #endregion

    }
}
