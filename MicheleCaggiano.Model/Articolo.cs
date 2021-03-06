//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MicheleCaggiano.Web.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Articolo
    {
        /* Constructor */
        public Articolo()
        { 
            this.Cancellato = false;
            this.Pubblicato = false;
            this.Commenti = new HashSet<Commento>();
            this.Categorie = new HashSet<Categoria>();
            this.AuthInfo = new AuthInfo();
        }
    
        public int Id { get; set; }
        public string Titolo { get; set; }
        public string Testo { get; set; }
        public bool Cancellato { get; set; }
        public bool Pubblicato { get; set; }
        public Nullable<System.DateTime> DataPubblicazione { get; set; }
    
        public AuthInfo AuthInfo { get; set; }
    
        public virtual ICollection<Commento> Commenti { get; set; }
        public virtual ICollection<Categoria> Categorie { get; set; }
    }
}
