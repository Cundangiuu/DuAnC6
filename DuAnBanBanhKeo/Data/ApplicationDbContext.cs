using DuAnBanBanhKeo.Api.Data.Entities;
using DuAnBanBanhKeo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DuAnBanBanhKeo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public virtual DbSet<AnhSanPham> AnhSanPhams { get; set; }
        public virtual DbSet<ChiTietCombo> ChiTietCombos { get; set; }
        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual DbSet<Combo> Combos { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<GiamGia> GiamGias { get; set; }
        public virtual DbSet<GioHang> GioHangs { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<MaNhapGiamGia> MaNhapGiamGias { get; set; }
        public virtual DbSet<NhaCungCapBanh> NhaCungCapBanhs { get; set; }
        public virtual DbSet<NhaCungCapKeo> NhaCungCapKeos { get; set; }
        public virtual DbSet<NhaCungCapNuocNgot> NhaCungCapNuocNgots { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ giữa SanPham và AnhSanPham
            modelBuilder.Entity<AnhSanPham>()
                .HasOne(a => a.MaSanPhamNavigation)
                .WithMany(s => s.AnhSanPhams)
                .HasForeignKey(a => a.MaSanPham);
            modelBuilder.Entity<ChiTietDonHang>()
                .HasOne(ctdh => ctdh.DonHang)
                .WithMany(dh => dh.ChiTietDonHangs)
                .HasForeignKey(ctdh => ctdh.MaDonHang)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SanPham>()
                .HasOne(s => s.MaNhaCungCapBanhNavigation)
                .WithMany(d => d.SanPhams)
                .HasForeignKey(s => s.MaNhaCungCapBanh);
            modelBuilder.Entity<SanPham>()
                .HasOne(s => s.MaNhaCungCapKeoNavigation)
                .WithMany(d => d.SanPhams)
                .HasForeignKey(s => s.MaNhaCungCapKeo);
            modelBuilder.Entity<SanPham>()
                .HasOne(s => s.MaNhaCungCapNuocNgotNavigation)
                .WithMany(d => d.SanPhams)
                .HasForeignKey(s => s.MaNhaCungCapNuocNgot);

            modelBuilder.Entity<TaiKhoan>()
               .HasOne(tk => tk.MaKhachHangNavigation)
               .WithMany(kh => kh.TaiKhoans)
               .HasForeignKey(tk => tk.MaKhachHang);

            // Cấu hình quan hệ giữa ChiTietComBo và ComBo
            modelBuilder.Entity<ChiTietCombo>()
                .HasOne(cc => cc.MaComboNavigation)
                .WithMany(c => c.ChiTietCombos)
                .HasForeignKey(cc => cc.MaCombo)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ giữa ChiTietComBo và SanPham
            modelBuilder.Entity<ChiTietCombo>()
                .HasOne(cc => cc.MaSanPhamNavigation)
                .WithMany(sp => sp.ChiTietCombos)
                .HasForeignKey(cc => cc.MaSanPham)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ giữa GiamGia và HoaDon
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.MaGiamGiaNavigation)
                .WithMany(g => g.HoaDons)
                .HasForeignKey(h => h.MaGiamGia)
                .OnDelete(DeleteBehavior.SetNull);

            // Cấu hình mối quan hệ giữa GiamGia và MaNhapGiamGia
            modelBuilder.Entity<MaNhapGiamGia>()
                .HasOne(m => m.GiamGia)
                .WithMany(g => g.MaNhapGiamGias)
                .HasForeignKey(m => m.MaGiamGia)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
