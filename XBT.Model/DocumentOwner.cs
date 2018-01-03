using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBT.Model
{
    /// <summary>
    /// The owner of the document. This class is the base class for different document owners e.g. customer, supplier etc. 
    /// </summary>
    public class DocumentOwner
    {
        #region "Public properties"

        [Key]
        public int Id { get; set; }

        #endregion

        #region "Associations"

        public List<Document> Documents { get; set; }

        #endregion
    }
}
