using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace XBT.Model
{
  /// <summary>
  /// Represents a customer
  /// </summary>
  [Table("Customers")]
  public class Customer : DocumentOwner
  {
    #region "public properties"

    [Key]
    public int CustomerNo { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public string Address { get; set; }

    #endregion
  }
}
