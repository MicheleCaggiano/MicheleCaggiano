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
    
    public partial class Categoria
    {
        /* Constructor */
        public Categoria()
        { 
            this.Articoli = new HashSet<Articolo>();
            this.AuthInfo = new AuthInfo();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
    
        public AuthInfo AuthInfo { get; set; }
    
        public virtual ICollection<Articolo> Articoli { get; set; }
    }
}
