using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace MicheleCaggiano.Models
{
  // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser
  {
    [Required(ErrorMessage = "Il campo Email è obbligatorio")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Il campo Nome è obbligatorio")]
    [Display(Name = "Nome")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Il campo Cognome è obbligatorio")]
    [Display(Name = "Cognome")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Il campo DefaultUserName è obbligatorio")]
    [Display(Name = "DefaultUserName")]
    public string DefaultUserName { get; set; }


    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
      // Add custom user claims here
      return userIdentity;

    }
  }
}