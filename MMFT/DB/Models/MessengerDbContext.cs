using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MMFT.DB.Models;

public partial class MessengerDbContext : DbContext
{
    public MessengerDbContext()
    {
    }

    public MessengerDbContext(DbContextOptions<MessengerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Nachrichten> Nachrichtens { get; set; }

    public virtual DbSet<Nutzer> Nutzers { get; set; }

    public virtual DbSet<PNutzer> PNutzers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=MessengerDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Nachrichten>(entity =>
        {
            entity.HasKey(e => new { e.Zeitstempel, e.SUuid, e.EUuid });

            entity.ToTable("Nachrichten");

            entity.HasIndex(e => new { e.EUuid, e.SUuid, e.Zeitstempel }, "Idx_Nachrichten").IsDescending(false, false, true);

            entity.Property(e => e.SUuid).HasColumnName("S_UUID");
            entity.Property(e => e.EUuid).HasColumnName("E_UUID");
            entity.Property(e => e.DInhalt).HasColumnName("D_Inhalt");
            entity.Property(e => e.TInhalt).HasColumnName("T_Inhalt");

            entity.HasOne(d => d.EUu).WithMany(p => p.NachrichtenEUus)
                .HasForeignKey(d => d.EUuid)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.SUu).WithMany(p => p.NachrichtenSUus)
                .HasForeignKey(d => d.SUuid)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Nutzer>(entity =>
        {
            entity.HasKey(e => e.Uuid);

            entity.ToTable("Nutzer");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.Ip)
                .IsRequired()
                .HasColumnName("IP");
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.PBild).HasColumnName("P_Bild");
            entity.Property(e => e.PublicKey)
                .IsRequired()
                .HasColumnName("Public_Key");
        });

        modelBuilder.Entity<PNutzer>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("P_Nutzer");

            entity.Property(e => e.KeyId).HasColumnName("Key_ID");
            entity.Property(e => e.PrivateKey)
                .IsRequired()
                .HasColumnName("Private_Key");
            entity.Property(e => e.Uuid)
                .IsRequired()
                .HasColumnName("UUID");

            entity.HasOne(d => d.Uu).WithMany()
                .HasForeignKey(d => d.Uuid)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
