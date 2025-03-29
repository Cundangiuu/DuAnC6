using DuAnBanBanhKeo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DuAnBanBanhKeo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<HoaDonNhap> HoaDonNhaps { get; set; }
        public DbSet<HoaDonXuat> HoaDonXuats { get; set; }
        public DbSet<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; }
        public DbSet<ChiTietHoaDonXuat> ChiTietHoaDonXuats { get; set; }
        public DbSet<KiemKe> KiemKes { get; set; }
        public DbSet<ChiTietKiemKe> ChiTietKiemKes { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public DbSet<HinhAnh> HinhAnhs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Thiết lập khóa chính cho các bảng có khóa không phải kiểu số
            modelBuilder.Entity<SanPham>().HasKey(sp => sp.MaSP);
            modelBuilder.Entity<NhaCungCap>().HasKey(ncc => ncc.MaNCC);
            modelBuilder.Entity<NhanVien>().HasKey(nv => nv.MaNV);
            modelBuilder.Entity<TaiKhoan>().HasKey(tk => tk.MaTK);
            modelBuilder.Entity<HoaDonNhap>().HasKey(hdn => hdn.MaHoaDonNhap);
            modelBuilder.Entity<HoaDonXuat>().HasKey(hdx => hdx.MaHoaDonXuat);
            modelBuilder.Entity<KhachHang>().HasKey(kh => kh.MaKH);
            modelBuilder.Entity<KiemKe>().HasKey(kk => kk.MaKiemKe);
            modelBuilder.Entity<PhieuNhap>().HasKey(pn => pn.MaPhieuNhap);
            modelBuilder.Entity<DanhMuc>().HasKey(dm => dm.MaDanhMuc);
            modelBuilder.Entity<HinhAnh>().HasKey(ha => ha.Id);

            //Thiết lập quan hệ 1-n giữa NhaCungCap và SanPham
            modelBuilder.Entity<SanPham>()
                .HasOne(sp => sp.NhaCungCap)
                .WithMany(ncc => ncc.SanPhams)
                .HasForeignKey(sp => sp.MaNCC);

            //Thiết lập quan hệ 1-n giữa DanhMuc và SanPham
            modelBuilder.Entity<SanPham>()
                .HasOne(sp => sp.DanhMuc)
                .WithMany(dm => dm.SanPhams)
                .HasForeignKey(sp => sp.MaDanhMuc);


            modelBuilder.Entity<HinhAnh>()
                .HasOne(ha => ha.SanPham)
                .WithMany(sp => sp.HinhAnhs)
                .HasForeignKey(ha => ha.MaSP);


            //Thiết lập quan hệ 1-n giữa NhanVien và HoaDonNhap
            modelBuilder.Entity<HoaDonNhap>()
                .HasOne(hdn => hdn.NhanVien)
                .WithMany(nv => nv.HoaDonNhaps)
                .HasForeignKey(hdn => hdn.MaNV);
            //Thiết lập quan hệ 1-n giữa NhaCungCap và HoaDonNhap
            modelBuilder.Entity<HoaDonNhap>()
                .HasOne(hdn => hdn.NhaCungCap)
                .WithMany(ncc => ncc.HoaDonNhaps)
                .HasForeignKey(hdn => hdn.MaNCC);

            //Thiết lập quan hệ 1-n giữa NhanVien và HoaDonXuat
            modelBuilder.Entity<HoaDonXuat>()
                .HasOne(hdx => hdx.NhanVien)
                .WithMany(nv => nv.HoaDonXuats)
                .HasForeignKey(hdx => hdx.MaNV);

            //Thiết lập quan hệ 1-n giữa NhanVien và KiemKe
            modelBuilder.Entity<KiemKe>()
                .HasOne(kk => kk.NhanVien)
                .WithMany(nv => nv.KiemKes)
                .HasForeignKey(kk => kk.MaNV);

            //Thiết lập quan hệ 1-n giữa KhachHang và HoaDonXuat
            modelBuilder.Entity<HoaDonXuat>()
                .HasOne(hdx => hdx.KhachHang)
                .WithMany(kh => kh.HoaDonXuats)
                .HasForeignKey(hdx => hdx.MaKH);

            //Thiết lập quan hệ 1-n giữa HoaDonNhap và ChiTietHoaDonNhap
            modelBuilder.Entity<ChiTietHoaDonNhap>()
                .HasOne(ct => ct.HoaDonNhap)
                .WithMany(hdn => hdn.ChiTietHoaDonNhaps)
                .HasForeignKey(ct => ct.MaHoaDonNhap);

            //Thiết lập quan hệ 1-n giữa HoaDonXuat và ChiTietHoaDonXuat
            modelBuilder.Entity<ChiTietHoaDonXuat>()
                .HasOne(ct => ct.HoaDonXuat)
                .WithMany(hdx => hdx.ChiTietHoaDonXuat)
                .HasForeignKey(ct => ct.MaHoaDonXuat);

            //Thiết lập quan hệ 1-n giữa KiemKe và ChiTietKiemKe
            modelBuilder.Entity<ChiTietKiemKe>()
                .HasOne(ct => ct.KiemKe)
                .WithMany(kk => kk.ChiTietKiemKes)
                .HasForeignKey(ct => ct.MaKiemKe);

            //Thiết lập quan hệ 1-n giữa PhieuNhap và ChiTietPhieuNhap
            modelBuilder.Entity<ChiTietPhieuNhap>()
                .HasOne(ct => ct.PhieuNhap)
                .WithMany(pn => pn.ChiTietPhieuNhaps)
                .HasForeignKey(ct => ct.MaPhieuNhap);

            //Thiết lập quan hệ 1-n giữa SanPham và ChiTietHoaDonNhap
            modelBuilder.Entity<ChiTietHoaDonNhap>()
                .HasOne(ct => ct.SanPham)
                .WithMany(sp => sp.ChiTietHoaDonNhaps)
                .HasForeignKey(ct => ct.MaSP);

            //Thiết lập quan hệ 1-n giữa SanPham và ChiTietHoaDonXuat
            modelBuilder.Entity<ChiTietHoaDonXuat>()
                .HasOne(ct => ct.SanPham)
                .WithMany(sp => sp.ChiTietHoaDonXuats)
                .HasForeignKey(ct => ct.MaSP);

            //Thiết lập quan hệ 1-n giữa SanPham và ChiTietKiemKe
            modelBuilder.Entity<ChiTietKiemKe>()
                .HasOne(ct => ct.SanPham)
                .WithMany(sp => sp.ChiTietKiemKes)
                .HasForeignKey(ct => ct.MaSP);

            //Thiết lập quan hệ 1-n giữa SanPham và ChiTietPhieuNhap
            modelBuilder.Entity<ChiTietPhieuNhap>()
                .HasOne(ct => ct.SanPham)
                .WithMany(sp => sp.ChiTietPhieuNhaps)
                .HasForeignKey(ct => ct.MaSP);

            //Thiết lập quan hệ 1-1 giữa NhanVien và TaiKhoan
            modelBuilder.Entity<NhanVien>()
                .HasOne(nv => nv.TaiKhoan)
                .WithOne(tk => tk.NhanVien)
                .HasForeignKey<TaiKhoan>(tk => tk.MaNV);



            //Seed Nhà Cung Cấp
            modelBuilder.Entity<NhaCungCap>().HasData(
                new NhaCungCap { MaNCC = "NCC001", TenNCC = "Coca-Cola VN", DiaChi = "Hà Nội", SoDienThoai = "0901123456" },
                new NhaCungCap { MaNCC = "NCC002", TenNCC = "PepsiCo VN", DiaChi = "TP. Hồ Chí Minh", SoDienThoai = "0902234567" },
                new NhaCungCap { MaNCC = "NCC003", TenNCC = "Mondelez VN", DiaChi = "Đà Nẵng", SoDienThoai = "0903345678" }
            );
            //Seed Sản Phẩm
            modelBuilder.Entity<SanPham>().HasData(
                new SanPham { MaSP = "SP001", TenSP = "Coca-Cola 330ml", GiaNhap = 5000, GiaBan = 10000, SoLuongTon = 100, DonViTinh = "Lon", MaNCC = "NCC001", MaDanhMuc = "DM003" },
                new SanPham { MaSP = "SP002", TenSP = "Pepsi 330ml", GiaNhap = 4800, GiaBan = 9500, SoLuongTon = 120, DonViTinh = "Lon", MaNCC = "NCC002", MaDanhMuc = "DM003" },
                new SanPham { MaSP = "SP003", TenSP = "Bánh Oreo", GiaNhap = 7000, GiaBan = 15000, SoLuongTon = 50, DonViTinh = "Gói", MaNCC = "NCC003", MaDanhMuc = "DM001" },
                new SanPham { MaSP = "SP004", TenSP = "Snack Lay's", GiaNhap = 9000, GiaBan = 18000, SoLuongTon = 75, DonViTinh = "Gói", MaNCC = "NCC001", MaDanhMuc = "DM002" }
            );
            //Seed Nhân Viên
            modelBuilder.Entity<NhanVien>().HasData(
                new NhanVien { MaNV = "NV001", HoTen = "Nguyễn Văn An", VaiTro = "Nhân viên", SoDienThoai = "0911123456", Email = "a.nguyen@example.com", TrangThai = true },
                new NhanVien { MaNV = "NV002", HoTen = "Trần Thị Bông", VaiTro = "Quản lý kho", SoDienThoai = "0912234567", Email = "b.tran@example.com", TrangThai = true },
                new NhanVien { MaNV = "NV003", HoTen = "Lê Văn Thuận", VaiTro = "Nhân viên", SoDienThoai = "0913345678", Email = "c.le@example.com", TrangThai = false }
            );
            //Seed Khách Hàng
            modelBuilder.Entity<KhachHang>().HasData(
                new KhachHang { MaKH = "KH001", TenKH = "Nguyễn An", SoDienThoai = "0987654321", DiaChi = "Hà Nội" },
                new KhachHang { MaKH = "KH002", TenKH = "Lê Bình", SoDienThoai = "0976543210", DiaChi = "Đà Nẵng" },
                new KhachHang { MaKH = "KH003", TenKH = "Trần Cường", SoDienThoai = "0965432109", DiaChi = "TP. HCM" }
            );
            //Seed Hóa Đơn Nhập
            modelBuilder.Entity<HoaDonNhap>().HasData(
                new HoaDonNhap { MaHoaDonNhap = "HDN001", MaNCC = "NCC001", MaNV = "NV001", NgayNhap = new DateTime(2024, 6, 1), TongTien = 2000000, TrangThai = 0 },
                new HoaDonNhap { MaHoaDonNhap = "HDN002", MaNCC = "NCC002", MaNV = "NV002", NgayNhap = new DateTime(2024, 6, 5), TongTien = 3500000, TrangThai = 0 }
            );

            //Seed Hóa Đơn Xuất
            modelBuilder.Entity<HoaDonXuat>().HasData(
                new HoaDonXuat { MaHoaDonXuat = "HDX001", MaKH = "KH001", MaNV = "NV002", NgayXuat = new DateTime(2024, 6, 10), TongTien = 500000, TrangThai = 0 },
                new HoaDonXuat { MaHoaDonXuat = "HDX002", MaKH = "KH002", MaNV = "NV001", NgayXuat = new DateTime(2024, 6, 15), TongTien = 750000, TrangThai = 0 }
            );

            // Seed dữ liệu cho bảng KiemKe
            modelBuilder.Entity<KiemKe>().HasData(
                new KiemKe { MaKiemKe = "KK001", MaNV = "NV003", NgayKiemKe = new DateTime(2024, 6, 20), GhiChu = "Kiểm kê định kỳ" },
                new KiemKe { MaKiemKe = "KK002", MaNV = "NV003", NgayKiemKe = new DateTime(2024, 6, 30), GhiChu = "Sai lệch số lượng" }
            );

            // Seed dữ liệu cho bảng ChiTietKiemKe (Ví dụ)
            modelBuilder.Entity<ChiTietKiemKe>().HasData(
                new ChiTietKiemKe { Id = 1, MaKiemKe = "KK001", MaSP = "SP001", SoLuongThucTe = 90, GhiChu = "Thiếu 10 bánh" },
                new ChiTietKiemKe { Id = 2, MaKiemKe = "KK001", MaSP = "SP002", SoLuongThucTe = 50, GhiChu = "" },
                new ChiTietKiemKe { Id = 3, MaKiemKe = "KK002", MaSP = "SP001", SoLuongThucTe = 80, GhiChu = "Thiếu 20 bánh" }
            );
            //Seed Tài Khoản
            modelBuilder.Entity<TaiKhoan>().HasData(
                new TaiKhoan { MaTK = "TK001", TenDangNhap = "user", MatKhau = "123", MaNV = "NV001", TrangThai = true, Email = "user@example.com" },
                new TaiKhoan { MaTK = "TK002", TenDangNhap = "admin", MatKhau = "123", MaNV = "NV002", TrangThai = true, Email = "nguyenhoanganh28052005@gmail.com" }
            );

            //Seed Danh Mục
            modelBuilder.Entity<DanhMuc>().HasData(
                new DanhMuc { MaDanhMuc = "DM001", TenDanhMuc = "Bánh", MoTa = "Các loại bánh ngọt và bánh mặn" },
                new DanhMuc { MaDanhMuc = "DM002", TenDanhMuc = "Kẹo", MoTa = "Các loại kẹo và snack" },
                new DanhMuc { MaDanhMuc = "DM003", TenDanhMuc = "Nước giải khát", MoTa = "Các loại nước ngọt và nước trái cây" }
            );
        }

    }
}