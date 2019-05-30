namespace WEBBANHANG.Models.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BanHangEntity : DbContext
    {
        public BanHangEntity()
            : base("name=BanHangEntity")
        {
        }

        public virtual DbSet<ChiTietDonDatHang> ChiTietDonDatHangs { get; set; }
        public virtual DbSet<DatDichVu> DatDichVus { get; set; }
        public virtual DbSet<DichVu> DichVus { get; set; }
        public virtual DbSet<DonDatHang> DonDatHangs { get; set; }
        public virtual DbSet<DonViTinh> DonViTinhs { get; set; }
        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public virtual DbSet<Medium> Media { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<ThuocTinh> ThuocTinhs { get; set; }
        public virtual DbSet<ThuocTinhSanPham> ThuocTinhSanPhams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DichVu>()
                .Property(e => e.AnhDaiDien)
                .IsUnicode(false);

            modelBuilder.Entity<DonDatHang>()
                .Property(e => e.SoHieuDon)
                .IsUnicode(false);

            modelBuilder.Entity<LoaiSanPham>()
                .Property(e => e.AnhDaiDien)
                .IsUnicode(false);

            modelBuilder.Entity<LoaiSanPham>()
                .HasMany(e => e.LoaiSanPham1)
                .WithOptional(e => e.LoaiSanPham2)
                .HasForeignKey(e => e.LoaiSanPhamPID);

            modelBuilder.Entity<Medium>()
                .Property(e => e.DuongLink)
                .IsUnicode(false);

            modelBuilder.Entity<NhaCungCap>()
                .Property(e => e.SoDienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<NhaCungCap>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.KyHieuSanPham)
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.AnhSanPham)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.PassWord)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.TaiKhoan)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DonDatHangs)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.TaiKhoanDatHangID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DonDatHangs1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.TaiKhoanNhanVienID);
        }
    }
}
