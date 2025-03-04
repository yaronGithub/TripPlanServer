using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TripPlanServer.Models;

public partial class TripPlanDbContext : DbContext
{
    public TripPlanDbContext()
    {
    }

    public TripPlanDbContext(DbContextOptions<TripPlanDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<PlanGroup> PlanGroups { get; set; }

    public virtual DbSet<PlanPlace> PlanPlaces { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=TripPlanDB;User ID=TripPlanAdminLogin;Password=kukuPassword;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BF04CD90A");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.PicId).HasName("PK__Pictures__B04A93C1B82F2810");

            entity.HasOne(d => d.Place).WithMany(p => p.Pictures).HasConstraintName("FK__Pictures__PlaceI__3D5E1FD2");

            entity.HasOne(d => d.Plan).WithMany(p => p.Pictures).HasConstraintName("FK__Pictures__PlanId__3C69FB99");

            entity.HasOne(d => d.PlanPlace).WithMany(p => p.Pictures).HasConstraintName("FK_PicturesPlan");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.PlaceId).HasName("PK__Places__D5222B6E74FA058A");

            entity.Property(e => e.PlaceId).ValueGeneratedNever();

            entity.HasOne(d => d.Category).WithMany(p => p.Places).HasConstraintName("FK__Places__Category__35BCFE0A");
        });

        modelBuilder.Entity<PlanGroup>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__PlanGrou__755C22B7AE811CEB");

            entity.HasOne(d => d.User).WithMany(p => p.PlanGroups).HasConstraintName("FK__PlanGroup__UserI__286302EC");

            entity.HasMany(d => d.Users).WithMany(p => p.Plans)
                .UsingEntity<Dictionary<string, object>>(
                    "Favorite",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favorites__UserI__4222D4EF"),
                    l => l.HasOne<PlanGroup>().WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favorites__PlanI__412EB0B6"),
                    j =>
                    {
                        j.HasKey("PlanId", "UserId").HasName("PK_FavoritesPlan");
                        j.ToTable("Favorites");
                    });

            entity.HasMany(d => d.UsersNavigation).WithMany(p => p.PlansNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "UserGroup",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserGroup__UserI__2D27B809"),
                    l => l.HasOne<PlanGroup>().WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserGroup__PlanI__2C3393D0"),
                    j =>
                    {
                        j.HasKey("PlanId", "UserId");
                        j.ToTable("UserGroup");
                    });
        });

        modelBuilder.Entity<PlanPlace>(entity =>
        {
            entity.HasOne(d => d.Place).WithMany(p => p.PlanPlaces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlanPlace__Place__38996AB5");

            entity.HasOne(d => d.Plan).WithMany(p => p.PlanPlaces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlanPlace__PlanI__398D8EEE");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79CEFDB54F37");

            entity.HasOne(d => d.Plan).WithMany(p => p.Reviews).HasConstraintName("FK__Reviews__PlanId__300424B4");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews).HasConstraintName("FK__Reviews__UserId__30F848ED");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C858E354D");

            entity.HasOne(d => d.Pic).WithMany(p => p.Users).HasConstraintName("FK__Users__PicId__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
