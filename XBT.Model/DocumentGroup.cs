using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBT.Model
{
  /// <summary>
  /// DocumentGroup categorises document types
  /// </summary>   
  public class DocumentGroup
  {
    #region "Constructors"

    public DocumentGroup()
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
