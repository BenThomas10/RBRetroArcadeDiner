using System;
using System.Collections.Generic;
using ArcadeDiner.Models;
using Microsoft.EntityFrameworkCore;

namespace ArcadeDiner.Data;

public partial class ArcadeDinerContext : DbContext
{
    public ArcadeDinerContext()
    {
    }

    public ArcadeDinerContext(DbContextOptions<ArcadeDinerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ReservationInfo> ReservationInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-BHFKT10\\SQLEXPRESS;Database=ArcadeDiner;TrustServerCertificate=True;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReservationInfo>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__B7EE5F24CED3DCC7");

            entity.ToTable("ReservationInfo");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.LastUpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PartyName).HasMaxLength(50);
            entity.Property(e => e.ReservationDate).HasColumnType("date");
            entity.Property(e => e.SubmissionDateTime).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
