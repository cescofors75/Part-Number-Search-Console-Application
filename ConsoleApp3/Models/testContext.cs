using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ConsoleApp3.Models
{
    public partial class testContext : DbContext
    {
        public testContext()
        {
        }

        public testContext(DbContextOptions<testContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArticleEan> ArticleEans { get; set; } = null!;
        public virtual DbSet<Eurocrossref2> Eurocrossref2s { get; set; } = null!;
        public virtual DbSet<EurocrossrefEan> EurocrossrefEans { get; set; } = null!;
        public virtual DbSet<Europroduct> Europroducts { get; set; } = null!;
       
        public virtual DbSet<StoreStocked> StoreStockeds { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=EUROLAB02-DELL\\DB_TEST; Database=test; User=sa; Password=");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleEan>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("articleEAN");

                entity.Property(e => e.Column1).HasColumnName("column1");

                entity.Property(e => e.Column2).HasColumnName("column2");
            });

            modelBuilder.Entity<Eurocrossref2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("eurocrossref2");

                entity.Property(e => e.Numoe).HasMaxLength(50);

                entity.Property(e => e.Reference).HasMaxLength(50);
            });

            modelBuilder.Entity<EurocrossrefEan>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("eurocrossref_ean");

                entity.Property(e => e.ArticleNumber).HasColumnName("articleNumber");

                entity.Property(e => e.AssemblyGroupNodeId).HasColumnName("assemblyGroupNodeId");

                entity.Property(e => e.Eancode).HasColumnName("eancode");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Lang).HasColumnName("lang");

                entity.Property(e => e.LegacyArticleId).HasColumnName("legacyArticleId");

                entity.Property(e => e.LegacyArticleId2).HasColumnName("legacyArticleId2");

                entity.Property(e => e.MfrId).HasColumnName("mfrId");

                entity.Property(e => e.RefEuro).HasColumnName("REF_EURO");

                entity.Property(e => e.RefFrn).HasColumnName("REF_FRN");

                entity.Property(e => e.ReferenceTypeDescription).HasColumnName("referenceTypeDescription");

                entity.Property(e => e.ReferenceTypeKey).HasColumnName("referenceTypeKey");
            });

            modelBuilder.Entity<Europroduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("europroducts");

                entity.Property(e => e.LibelleProduit)
                    .HasMaxLength(100)
                    .HasColumnName("Libelle_produit");

                entity.Property(e => e.PrixEuroHt).HasColumnName("Prix_Euro_HT");

                entity.Property(e => e.Reference).HasMaxLength(50);
            });

           


            modelBuilder.Entity<StoreStocked>(entity =>
            {
                entity.HasKey(e => e.IdNew)
                    .HasName("PK__storeSto__54A5F6F4CAE14AF6");

                entity.ToTable("storeStocked");

                entity.Property(e => e.IdNew).HasColumnName("Id_new");

                entity.Property(e => e.DateStoked)
                    .HasColumnType("datetime")
                    .HasColumnName("dateStoked");

                entity.Property(e => e.DateUptade)
                    .HasColumnType("datetime")
                    .HasColumnName("dateUptade");

                entity.Property(e => e.RefEuro)
                    .HasMaxLength(10)
                    .HasColumnName("refEuro")
                    .IsFixedLength();

                entity.Property(e => e.StoreCountry)
                    .HasMaxLength(10)
                    .HasColumnName("storeCountry")
                    .IsFixedLength();

                entity.Property(e => e.StoreName)
                    .HasMaxLength(100)
                    .HasColumnName("storeName")
                    .IsFixedLength();

                entity.Property(e => e.Storeprice).HasColumnName("storeprice");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
