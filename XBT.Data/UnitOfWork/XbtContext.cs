using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using XBT.Model;

namespace XBT.Data
{
    public class XbtContext : DbContext
    {
        public DbSet<DocumentGroup> DocumentGroups { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentTag> DocumentTags { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentOwner> DocumentOwners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Map one-to-many document self referencing entity. Childitems will be used to track document versions
            modelBuilder.Entity<Document>()
                          .HasOptional(u => u.ParentDocument)
                          .WithMany(u => u.ChildDocuments)
                          .HasForeignKey(u => u.ParentDocumentId);

            modelBuilder.Entity<DocumentType>()
                  .HasMany(t => t.DocumentTags)
                  .WithMany(u => u.DocumentTypes)
                  .Map(c =>
                  {
                      c.ToTable("DocumentTagDocumentType");
                      c.MapLeftKey("DocumentTypeId");
                      c.MapRightKey("DocumentTagId");

                  });

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
