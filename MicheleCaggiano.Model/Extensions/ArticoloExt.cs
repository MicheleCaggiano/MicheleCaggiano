using System.Linq;

namespace MicheleCaggiano.Web.Model
{
  public partial class Articolo
  {
    public void BindCategorie(string Ids, ModelContainer ctx)
    {
      if (!string.IsNullOrEmpty(Ids) && ctx != null)
      {
        this.Categorie.Clear();
        var IdsToCheck = Ids.Split(',');
        var categorie = ctx.Categoria.Where(cat => IdsToCheck.Contains(cat.Id.ToString()));
        // Effettua il bind
        this.Categorie = categorie.ToList();
      }
    }
  }
}

