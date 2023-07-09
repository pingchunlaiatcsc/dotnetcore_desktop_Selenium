using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore_desktop_app.Models;

public partial class DUsersHappyaimonkeySourceReposDotnetcoreDesktopAppDataMydbMdfContext : DbContext
{
    public DUsersHappyaimonkeySourceReposDotnetcoreDesktopAppDataMydbMdfContext()
    {
    }

    public DUsersHappyaimonkeySourceReposDotnetcoreDesktopAppDataMydbMdfContext(DbContextOptions<DUsersHappyaimonkeySourceReposDotnetcoreDesktopAppDataMydbMdfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TCustomer> TCustomers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=MyDbConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TCustomer>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tmp_ms_x__D9F8227CD923E939");

            entity.ToTable("tCustomer");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAddress)
                .HasMaxLength(50)
                .HasColumnName("fAddress");
            entity.Property(e => e.FEmail)
                .HasMaxLength(50)
                .HasColumnName("fEmail");
            entity.Property(e => e.FName)
                .HasMaxLength(50)
                .HasColumnName("fName");
            entity.Property(e => e.FPhone)
                .HasMaxLength(50)
                .HasColumnName("fPhone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
