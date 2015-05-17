using System.ComponentModel.DataAnnotations;

namespace MicheleCaggiano.Web.Models
{
  public class EmailViewModel
  {
    [Required]
    public string Nome { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Oggetto { get; set; }
    [Required]
    public string Messaggio { get; set; }

  }
}