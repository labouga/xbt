using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBT.Model
{
  /// <summary>
  /// A Tag allows documents to be sub categorized based on the tags attached to them. 
  /// </summary>
  public class DocumentTag
  {
    #region "Constructors"

    public DocumentTag()
    {
      DateCreated = DateTime.Now.ToLocalTime();
    }

    #endregion

    #region "Public properties"

    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public DateTime DateCreated { get; set; }
    
    #endregion

    #region "Associations"

    public List<DocumentType> DocumentTypes { get; set; }

    #endregion
  }
}
