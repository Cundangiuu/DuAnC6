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
            modelBuilder.Entity<SanPham>().HasData(
            new SanPham
            {
                MaSanPham = "SP001",
                TenSanPham = "Bánh Kem Sôcôla",
                Gia = 150000m,
                MoTa = "Bánh kem sôcôla tươi ngon",
                SoLuong = 100,
                NgayThem = DateOnly.FromDateTime(DateTime.Now),
                DonVi = "Cái",
                Nsx = DateOnly.FromDateTime(DateTime.Now.AddMonths(-3)),
                Hsd = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                TrangThai = 1
            },
            new SanPham
            {
                MaSanPham = "SP002",
                TenSanPham = "Kẹo Dẻo",
                Gia = 50000m,
                MoTa = "Kẹo dẻo ngon miệng cho mọi lứa tuổi",
                SoLuong = 200,
                NgayThem = DateOnly.FromDateTime(DateTime.Now),
                DonVi = "Hộp",
                Nsx = DateOnly.FromDateTime(DateTime.Now.AddMonths(-2)),
                Hsd = DateOnly.FromDateTime(DateTime.Now.AddMonths(12)),
                TrangThai = 1
            },
            new SanPham
            {
                MaSanPham = "SP003",
                TenSanPham = "Nước Ngọt Cola",
                Gia = 10000m,
                MoTa = "Nước ngọt cola cho mùa hè mát lạnh",
                SoLuong = 300,
                NgayThem = DateOnly.FromDateTime(DateTime.Now),
                DonVi = "Lít",
                Nsx = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                Hsd = DateOnly.FromDateTime(DateTime.Now.AddMonths(8)),
                TrangThai = 1
            });
            modelBuilder.Entity<Combo>().HasData(
            new Combo
            {
                MaCombo = "C001",
                TenCombo = "Combo Bánh & Kẹo",
                MoTa = "Combo gồm 1 bánh kem sôcôla và 2 hộp kẹo dẻo",
                Gia = 200000m,
                TrangThai = 1,
                Anh = "combo-banh-keo.jpg", // Example image path
                SoLuongCombo = 50
            },
            new Combo
            {
                MaCombo = "C002",
                TenCombo = "Combo Nước Ngọt & Bánh",
                MoTa = "Combo gồm 1 bánh kem sôcôla và 1 nước ngọt Cola",
                Gia = 180000m,
                TrangThai = 1,
                Anh = "combo-nuoc-ngot.jpg", // Example image path
                SoLuongCombo = 80
            });

            modelBuilder.Entity<ChiTietCombo>().HasData(
                new ChiTietCombo
                {
                    Id = 1,
                    MaCombo = "C001",
                    MaSanPham = "SP001" 
                },
                new ChiTietCombo
                {
                    Id = 2,
                    MaCombo = "C001",
                    MaSanPham = "SP002" 
                },
                new ChiTietCombo
                {
                    Id = 3,
                    MaCombo = "C002",
                    MaSanPham = "SP001" 
                },
                new ChiTietCombo
                {
                    Id = 4,
                    MaCombo = "C002",
                    MaSanPham = "SP002"
                });
        }
    }

}
