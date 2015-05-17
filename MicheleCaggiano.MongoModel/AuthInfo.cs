
namespace MicheleCaggiano.MongoModel
{
  using MongoDB.Bson;
  using MongoDB.Bson.Serialization.Attributes;
  using System.Collections.Generic;
  using System.Linq;
  using System.ComponentModel.DataAnnotations;

  public partial class AuthInfo
  {
    [Required]
    [Display(Name = "Creato da")]
    public string CreatedBy { get; set; }
    [Required]
    [Display(Name = "Modificato da")]
    public string ModifiedBy { get; set; }
    [Required]
    [Display(Name = "Data creazione")]
    public System.DateTime Created { get; set; }
    [Required]
    [Display(Name = "Data ultima modifica")]
    public System.DateTime Modified { get; set; }
    [Required]
    public string UserId { get; set; }

  }
}
