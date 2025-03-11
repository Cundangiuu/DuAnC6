using System.ComponentModel.DataAnnotations;

namespace DuAnBanBanhKeo.Data.Entities
{
    public class NhanVien
    {
        [Key]
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string VaiTro { get; set; } // Ví dụ: "Nhân viên kho", "Quản lý kho"
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public ICollection<KiemKe> KiemKes { get; set; } = new List<KiemKe>();
        public bool TrangThai { get; set; } = true; // true = Đang hoạt động, false = Không hoạt động
        public TaiKhoan TaiKhoan { get; set; }
        public ICollection<HoaDonXuat> HoaDonXuats { get; set; } = new List<HoaDonXuat>();
        public ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();
    }

}
