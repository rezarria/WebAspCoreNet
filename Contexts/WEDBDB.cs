using Microsoft.EntityFrameworkCore;

namespace Web.Contexts;

public class WEBDbContext : DbContext
{
    public WEBDbContext(DbContextOptions<WEBDbContext> options) : base(options) { }


    public DbSet<Models.TaiKhoan> TaiKhoan { get; set; } = null!;
    public DbSet<Models.BaiViet> BaiViet { get; set; } = null!;
    public DbSet<Models.Tep> Tep { get; set; } = null!;
    public DbSet<Models.TepTaiKhoan> TepTaiKhoan { get; set; } = null!;
    public DbSet<Models.Volume> Volume { get; set; } = null!;
    public DbSet<Models.VolumeUploadAllow> VolumeUploadAllow { get; set; } = null!;
    public DbSet<Models.VolumeUploadDeny> VolumeUploadDeny { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.TaiKhoan>().HasIndex(obj => obj.Username).IsUnique();

        modelBuilder.Entity<Models.Volume>(entity =>
            {
                entity.Property(e => e.DirectorySeparatorChar)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

        modelBuilder.Entity<Models.Tep>(entity =>
       {
           entity.HasOne(d => d.TaiKhoanSoHuu)
               .WithMany(p => p.DanhSachTep)
               .HasForeignKey(d => d.IdOwner)
               .OnDelete(DeleteBehavior.ClientSetNull);
       });

        modelBuilder.Entity<Models.TepTaiKhoan>(entity =>
        {
            entity.HasKey(e => new { e.IdTep, e.IdTaiKhoan });

            entity.HasOne(d => d.TaiKhoan)
                .WithMany(p => p.QuyenHanTep)
                .HasForeignKey(d => d.IdTaiKhoan);

            entity.HasOne(d => d.Tep)
                .WithMany(p => p.QuyenHanTaiKhoan)
                .HasForeignKey(d => d.IdTep);
        });

        modelBuilder.Entity<Models.VolumeUploadAllow>(entity =>
        {
            entity.HasOne(d => d.Volume)
                .WithMany(p => p.VolumeUploadAllows)
                .HasForeignKey(d => d.IdVolume);
        });

        modelBuilder.Entity<Models.VolumeUploadDeny>(entity =>
        {
            entity.HasOne(d => d.Volume)
                .WithMany(p => p.VolumeUploadDenies)
                .HasForeignKey(d => d.IdVolume);
        });

    }
}