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
    
    public partial class Commento
    {
        /* Constructor */
        public Commento()
        { 
            this.Nascosto = false;
            this.Cancellato = false;
            this.CommentoPadre = new HashSet<Commento>();
            this.AuthInfo = new AuthInfo();
        }
    
        public int Id { get; set; }
        public string Testo { get; set; }
        public int CommentoId { get; set; }
        public bool Nascosto { get; set; }
        public bool Cancellato { get; set; }
        public int ArticoloId { get; set; }
    
        public AuthInfo AuthInfo { get; set; }
    
        public virtual ICollection<Commento> CommentoPadre { get; set; }
        public virtual Commento Commenti { get; set; }
        public virtual Articolo Articolo { get; set; }
    }
}