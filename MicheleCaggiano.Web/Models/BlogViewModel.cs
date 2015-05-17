using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MicheleCaggiano.Web.Model;

namespace MicheleCaggiano.Models
{
  public class BlogViewModel
  {
    public enum TipologiaArticoli
    {
      Pubblicati = 0,
      Bozze = 1,
      Tutti = 2
    }

    public IQueryable<Articolo> Articoli;
    public TipologiaArticoli TipoArticoli;
    public int ArticoliPubblicati;
    public int Bozze;
    public int NumeroCategorie;
    public string TestoRicerca;

    public BlogViewModel()
    {
      this.TipoArticoli = TipologiaArticoli.Pubblicati;
    }
  }
}