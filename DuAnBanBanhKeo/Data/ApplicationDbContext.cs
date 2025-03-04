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
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
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
            // Cấu hình quan hệ giữa SanPham và NhaCungCap
            modelBuilder.Entity<SanPham>()
                .HasOne(s => s.MaNhaCungCapNavigation)
                .WithMany(d => d.SanPhams)
                .HasForeignKey(s => s.MaNhaCungCap);
            modelBuilder.Entity<TaiKhoan>()
               .HasOne(tk => tk.MaKhachHangNavigation)
               .WithMany(kh => kh.TaiKhoans)
               .HasForeignKey(tk => tk.MaKhachHang);

            // Cấu hình quan hệ giữa ChiTietComBo và ComBo
            modelBuilder.Entity<ChiTietCombo>()
                .HasOne(cc => cc.MaComboNavigation)
                .WithMany(c => c.ChiTietCombos)
                .HasForeignKey(cc => cc.MaCombo)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Cascade nếu cần

            // Cấu hình quan hệ giữa ChiTietComBo và SanPham
            modelBuilder.Entity<ChiTietCombo>()
                .HasOne(cc => cc.MaSanPhamNavigation)
                .WithMany(sp => sp.ChiTietCombos)
                .HasForeignKey(cc => cc.MaSanPham)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Cascade nếu cần

            // Cấu hình quan hệ giữa GiamGia và HoaDon
            modelBuilder.Entity<HoaDon>()
                .HasOne(h => h.MaGiamGiaNavigation) // Thiết lập mối quan hệ từ HoaDon đến GiamGia
                .WithMany(g => g.HoaDons)           // Một GiamGia có thể liên kết với nhiều HoaDon
                .HasForeignKey(h => h.MaGiamGia)    // Khóa ngoại là MaGiamGia
                .OnDelete(DeleteBehavior.SetNull);  // Xóa mềm: nếu GiamGia bị xóa, MaGiamGia sẽ được set null

            // Cấu hình mối quan hệ giữa GiamGia và MaNhapGiamGia
            modelBuilder.Entity<MaNhapGiamGia>()
                .HasOne(m => m.GiamGia) // Một MaNhapGiamGia liên kết với một GiamGia
                .WithMany(g => g.MaNhapGiamGias) // Một GiamGia có nhiều MaNhapGiamGias
                .HasForeignKey(m => m.MaGiamGia) // Khóa ngoại là MaGiamGia
                .OnDelete(DeleteBehavior.Cascade); // Xóa mã giảm giá sẽ xóa cả mã nhập liên quan
        }


    }
}
