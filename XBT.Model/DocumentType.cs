using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBT.Model
{
    /// <summary>
    /// DocumentType categorises documents
    /// </summary>
    public class DocumentType
    {
        #region "Constructors"

        public DocumentType()
        {
            DateCreated = DateTime.Now.ToLocalTime();
        }

        #endregion

        #region "Public Properties"

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string DocumentDirectory { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        #endregion

        #region "Associations"

        [Required]
        public DocumentGroup DocumentGroup { get; set; }

        public List<Document> Documents { get; set; }
        public List<DocumentTag> DocumentTags { get; set; }

        #endregion
    }

}
