﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ModelContainer : DbContext
    {
        public ModelContainer() // : base("name=ModelContainer")
          : base(System.Configuration.ConfigurationManager.ConnectionStrings["ModelContainer"].ConnectionString)
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Articolo> Articolo { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Commento> Commento { get; set; }
    }
}
